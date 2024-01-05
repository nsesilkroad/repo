using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Silkroad.Core.Base.Const;
using Silkroad.Core.Base.Enumeration.ExtTypes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Silkroad.Modules.ComplianceManagement.Manager
{
    public static class ComplianceManager
    {
        /// <summary>
        ///     Logger
        /// </summary>
        private static readonly ILogger Logger = LoggerManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Compliance

        /// <summary>
        ///     Compliance 데이터 추가
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="data"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static Compliance AddCompliance(this IComplianceManagementDbContext dbContext, Compliance data, BaseMember member)
        {
            try
            {
                var trackerData = dbContext.GetTracker(data.trackerId);
                data.TrackerType = trackerData;
                data.Writer = member;
                data.WriteDateTime = DateTime.Now;
                dbContext.Compliances.Add(data);

                dbContext.SaveChanges();

                return data;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Compliance 수정
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="updateData">수정할 Compliance</param>
        /// <param name="member">유저 정보</param>
        /// <param name="attachTarget">수정된 첨부파일 정보</param>
        /// <returns></returns>
        public static Compliance UpdateCompliance(this IComplianceManagementDbContext dbContext, Compliance updateData, BaseMember member, Attachment attachTarget = null)
        {
            try
            {
                
                updateData.Writer = member;
                updateData.WriteDateTime = DateTime.Now;
                dbContext.SaveChanges();

                return updateData;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Compliance 삭제
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="id">Compliance Id</param>
        /// <param name="member">유저 정보</param>
        /// <returns>삭제한 Compliance 데이터</returns>
        public static Compliance RemoveCompliance(this IComplianceManagementDbContext dbContext, int id, BaseMember member)
        {
            try
            {
                var target = dbContext.Compliances
                    .FirstOrDefault(row => !row.IsDeleted && row.Id == id);

                if (target != null)
                {
                    target.IsDeleted = true;
                    target.WriteDateTime = DateTime.Now;
                    target.Writer = member;

                    dbContext.SaveChanges();
                }

                return target;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        #endregion

        #region CompliancePhase

        /// <summary>
        ///     CompliancePhase 추가
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="phaseData">CompliancePhase 데이터</param>
        /// <param name="trackerId">CompliancePhase를 추가할 매뉴의 TrackerId</param>
        /// <param name="member">유저 정보</param>
        /// <returns></returns>
        public static Compliance AddCompliancePhase(this IComplianceManagementDbContext dbContext, Compliance phaseData, string trackerId, BaseMember member)
        {
            try
            {
                var trackerData = dbContext.GetTracker(trackerId);
                phaseData.TrackerType = trackerData;
                phaseData.Writer = member;
                phaseData.WriteDateTime = DateTime.Now;
                phaseData.Project = dbContext.GetProject(trackerId);
                dbContext.Compliances.Add(phaseData);
                dbContext.SaveChanges();
                return phaseData;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        // TODO: Read

        // TODO: Update

        /// <summary>
        ///     Compliance Phase 삭제
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="id">삭제할 Phase Id</param>
        /// <param name="member">유저 정보</param>
        /// <returns></returns>
        public static Compliance RemoveCompliancePhase(this IComplianceManagementDbContext dbContext, int id, BaseMember member)
        {
            try
            {
                var target = dbContext.Compliances
                    .Include(c => c.TrackerType)
                    .Include(c => c.ChildModels)
                    .FirstOrDefault(row => row.Id == id);

                if (target != null)
                {
                    // 자식 문서 삭제
                    foreach (var compliance in target.ChildModels)
                    {
                        compliance.IsDeleted = true;
                        compliance.WriteDateTime = DateTime.Now;
                        compliance.Writer = member;
                    }

                    target.IsDeleted = true;
                    target.WriteDateTime = DateTime.Now;
                    target.Writer = member;

                    dbContext.SaveChanges();
                }

                return target;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        #endregion

        #region Compliance Tree, List

        /// <summary>
        ///     Compliance 목록 로드(Phase와 포함된 Compliance목록 형태)
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="trackerId">트래커 정보</param>
        /// <param name="type">Compliance Type</param>
        /// <param name="member">유저 정보</param>
        /// <returns>Compliance Phase목록</returns>
        public static List<Compliance> GetComplianceDataList(this IComplianceManagementDbContext dbContext,
            string trackerId, EComplianceCategoryType type, BaseMember member)
        {
            try
            {
                var trackerTypeName = TrackerTypeMappers.GetNewTrackerType(trackerId.GetTrackerType());
                var menuId = trackerId.GetTrackerIntegerNumber();

                var result = dbContext.Compliances
                    .Include(d => d.TrackerType)
                    .Include(c => c.Attachments)
                    .Where(row => !row.IsDeleted
                                  && (row.TrackerType.Name == trackerTypeName || row.TrackerType.DisplayName == trackerTypeName)
                                  && (menuId == 0 ? row.TrackerType.MenuId == null : row.TrackerType.MenuId == menuId)
                                  && row.ComplianceCategory == type);

                if (!result.Any())
                {
                    var templateData = dbContext.GetComplianceTemplate(type);
                    dbContext.CreateComplianceTree(trackerId, type, templateData, member);
                }

                var allList = result.ToList();
                var rootList = allList
                    .Where(row => row.ParentId == null)
                    .Select(row => new Compliance(row.Name)
                    {
                        Description = row.Description,
                        FileName = row.FileName,
                        Id = row.Id,
                        TagName = row.TagName,
                        ComplianceCategory = row.ComplianceCategory,
                        Attachments = row.Attachments,
                        ChildModels = new List<BaseModel>(),
                        IsDeleted = row.IsDeleted,
                        ParentId = row.ParentId,
                        Index = row.Index,
                        TrackerTypeId = row.TrackerTypeId
                    })
                    .OrderBy(row => row.Index)
                    .ToList();

                foreach (var root in rootList)
                {
                    root.ChildModels = allList.Where(row => row.ParentId == root.Id).OrderBy(row => row.Index).OfType<BaseModel>().ToList();
                }

                return rootList;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Compliance Tree 로드
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="trackerId">불러올 Compliance 매뉴 TrackerId</param>
        /// <param name="type">로드할 Compliance 목록 타입(특정 탭)</param>
        /// <param name="member">유저 정보</param>
        /// <returns>트리 루트</returns>
        public static List<Compliance> GetComplianceTree(this IComplianceManagementDbContext dbContext,
            string trackerId, EComplianceCategoryType type, BaseMember member)
        {
            try
            {
                return dbContext.GetComplianceDataList(trackerId, type, member);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Compliance Tree 생성
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="trackerId">트래커 정보</param>
        /// <param name="type">생성할 Compliance 탭 종류</param>
        /// <param name="templateList">템플릿 정보</param>
        /// <param name="member">유저 정보</param>
        /// <returns>Compliance Tree</returns>
        public static void CreateComplianceTree(this IComplianceManagementDbContext dbContext,
            string trackerId, EComplianceCategoryType type, List<ComplianceTemplate> templateList, BaseMember member)
        {
            try
            {
                //// template 데이터 compliance 테이블에 추가

                var phaseList = templateList
                    .Where(row => row.ParentComplianceTemplate == null)
                    .OrderBy(row => row.Index)
                    .ToList();
                var project = dbContext.GetProject(trackerId);
                var trackerType = dbContext.GetTracker(trackerId);

                // phase
                foreach (var phase in phaseList)
                {
                    var newPhase = new Compliance(phase.Name)
                    {
                        FileName = phase.FileName,
                        ComplianceCategory = phase.ComplianceCategory,
                        TagName = phase.TagName,
                        Index = phase.Index,
                        Writer = member,
                        WriteDateTime = DateTime.Now,
                        TrackerType = trackerType,
                        Project = project
                    };

                    // document
                    foreach (var complianceTemplate in phase.ComplianceTemplateList)
                    {
                        var newCompliance = new Compliance(complianceTemplate.Name)
                        {
                            FileName = complianceTemplate.FileName,
                            ComplianceCategory = complianceTemplate.ComplianceCategory,
                            TagName = complianceTemplate.TagName,
                            Index = complianceTemplate.Index,
                            Writer = member,
                            WriteDateTime = DateTime.Now,
                            TrackerType = trackerType,
                            Parent = newPhase,
                            Project = project
                        };

                        // 파일이 있는 문서의 경우 Attachment에 추가해야 함
                        // docx
                        dbContext.CopyTemplateFileToCompliance(
                            newCompliance, trackerId, member, false,EComplianceDocumentType.Docx.ToDisplayText());
                        // pdf
                        dbContext.CopyTemplateFileToCompliance(
                            newCompliance, trackerId, member, false, EComplianceDocumentType.Pdf.ToDisplayText());

                        dbContext.Compliances.Add(newCompliance);
                    }

                    dbContext.Compliances.Add(newPhase);
                }

                dbContext.SaveChanges();

                //// 무한루프 방지 목적으로 GetComplianceTree() 사용하지 않음
                //var treeData = dbContext.GetComplianceDataList(trackerId, type, member);
                
                //return treeData;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Compliance Tree 초기화
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="trackerId">트래커 정보</param>
        /// <param name="type">Compliance 탭 종류</param>
        /// <param name="member">유저 정보</param>
        /// <returns>초기화한 Compliance Tree</returns>
        public static List<Compliance> RestoreComplianceTree(this IComplianceManagementDbContext dbContext,
            string trackerId, EComplianceCategoryType type, BaseMember member)
        {
            try
            {
                var trackerTypeName = TrackerTypeMappers.GetNewTrackerType(trackerId.GetTrackerType());
                var menuId = trackerId.GetTrackerIntegerNumber();

                // 해당 탭의 기존 Compliance 제거
                var currentCompliances = dbContext.Compliances
                    .Include(data => data.TrackerType)
                    .Where(row => (row.TrackerType.Name == trackerTypeName || row.TrackerType.DisplayName == trackerTypeName) 
                                  && (menuId == 0 ? row.TrackerType.MenuId == null : row.TrackerType.MenuId == menuId) 
                                  && row.ComplianceCategory == type);

                foreach (var compliance in currentCompliances)
                {
                    compliance.IsDeleted = true;
                }

                dbContext.SaveChanges();

                // CreateComplianceTree(type) 로 생성
                //var templateCompliances = dbContext.GetComplianceTemplate(type);
                //return dbContext.CreateComplianceTree(trackerId, type, templateCompliances, member);

                return dbContext.GetComplianceDataList(trackerId, type, member);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        #endregion

    }
}
