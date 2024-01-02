using Aspose.Words;
using Aspose.Words.Saving;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Silkroad.Core.Base.Const.Resource;
using Silkroad.Core.Base.Helper;
using Silkroad.Core.Base.Manager;
using Silkroad.Core.Base.Model;
using Silkroad.Modules.ComplianceManagement.Const;
using Silkroad.Modules.ComplianceManagement.Model;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Silkroad.Modules.ComplianceManagement.Manager
{
    public static class ComplianceFileManager
    {
        /// <summary>
        ///     Logger
        /// </summary>
        private static readonly ILogger Logger = LoggerManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     Compliance 파일 복사
        /// </summary>
        /// <param name="srcPath">원본 파일 경로</param>
        /// <param name="dstPath">생성 파일 경로</param>
        /// <returns>성공여부</returns>
        public static bool ComplianceFileCopy(string srcPath, string dstPath)
        {
            try
            {
                using (var templateStream = new FileStream(srcPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var targetStream = new FileStream(dstPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                    {
                        const int bufferLen = 65000;
                        var buffer = new byte[bufferLen];
                        int count;
                        while ((count = templateStream.Read(buffer, 0, bufferLen)) > 0)
                        {
                            targetStream.Write(buffer, 0, count);
                        }
                        targetStream.Close();
                    }
                    templateStream.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return false;
            }

        }

        /// <summary>
        ///     Compliance Template 파일을 해당 트래커 폴더로 복사
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="compliance">연결할 Compliance 정보</param>
        /// <param name="trackerId">트래커 정보</param>
        /// <param name="member">유저 정보</param>
        /// <param name="saveChanged">db 저장 여부</param>
        /// <param name="docType">파일 확장자 필요 시 설정</param>
        /// <returns>생성 여부</returns>
        public static bool CopyTemplateFileToCompliance(this IComplianceManagementDbContext dbContext,
            Compliance compliance, string trackerId, BaseMember member, bool saveChanged = true, string docType = "")
        {
            try
            {
                // 파일 확장자 처리
                if (!string.IsNullOrEmpty(docType))
                {
                    docType = $".{docType}";
                }


                //// Directory Setting

                // 파일 명
                if (string.IsNullOrEmpty(compliance.FileName))
                {
                    return false;
                }
                var fileName = $"{compliance.FileName.Replace("\r\n", "")}{docType}";

                // Template 폴더 위치
                var templateTypeFullPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    Path.Combine(DefaultPath.TemplatePath, ConstCompliancePath.TemplatePath),
                    compliance.ComplianceCategory.ToDisplayText());

                // 복사할 트래커 폴더 위치
                var trackerPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    ConstCompliancePath.TrackerPath,
                    trackerId,
                    compliance.ComplianceCategory.ToDisplayText());

                // 저장할 Path 값
                var attachPath = Path.Combine(
                    ConstCompliancePath.CurrentDirectorySymbol,
                    ConstCompliancePath.TrackerPath,
                    trackerId,
                    compliance.ComplianceCategory.ToDisplayText(),
                    fileName);

                // 폴더 없으면 생성
                if (!Directory.Exists(trackerPath))
                {
                    Directory.CreateDirectory(trackerPath);
                }

                // ComplianceTemplate 파일 전체경로
                var fileFullPath = Path.Combine(templateTypeFullPath, fileName);
                // Compliance 파일 전체경로
                var trackerFileFullPath = Path.Combine(trackerPath, fileName);

                //// fileCopy
                
                // 기존파일 제거
                var prev = new FileInfo(trackerFileFullPath);
                if (prev.Exists) prev.Delete();
                ComplianceFileCopy(fileFullPath, trackerFileFullPath);

                //// Create AttachmentData, Compliance-Attachment Relation
                var docFileInfo = new FileInfo(fileFullPath);
                var newAttachment = new Attachment(EFileCategory.Publication, fileName, attachPath, docFileInfo.Length, "",
                    FileHelper.GetChecksum(docFileInfo), member, DateTime.Now);
                newAttachment.TargetData = compliance;
                newAttachment.TargetProject = compliance.Project;
                compliance.Attachments.Add(newAttachment);

                dbContext.Attachments.Add(newAttachment);


                // save option
                if (saveChanged)
                {
                    dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     모델의 docx 파일 정보로 pdf 생성
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="model"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool CreatePdf(this IComplianceManagementDbContext dbContext, BaseModel model, BaseMember member, bool saveChanges = true)
        {
            try
            {
                // Attach Load
                if (model.Id > 0 && !model.Attachments.Any())
                {
                    dbContext.Compliances
                        .Include(c => c.Attachments)
                        .FirstOrDefault(c => c.Id == model.Id);
                }

                // 변환대상인지
                Func<Attachment, bool> hasDocx = (attach) => (
                    !attach.IsDeleted &&
                    Path.GetExtension(attach.OriginalFileName).Equals(".docx") &&
                    attach.Category == EFileCategory.Publication
                );
                if (!model.Attachments.Any(hasDocx)) return false;

                // 변환
                var attachId = model.Attachments.FirstOrDefault(hasDocx).Id;
                return dbContext.createPdf(attachId, member, model, saveChanges);
            }
            catch (Exception e)
            {
                Logger.LogError("Compliance PDF 변환 실패");
                Logger.LogError(e, e.Message);
                throw;
            }
        }

        /// <summary>
        ///     docx 파일을 pdf 파일로 변환
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="attachId"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool createPdf(this IComplianceManagementDbContext dbContext, int attachId, BaseMember member, BaseModel model, bool saveChanges = true)
        {
            try
            {
                var attachment = dbContext.Attachments.FirstOrDefault(row => row.Id == attachId && !row.IsDeleted);
                // 변환할 원본이 없음
                if (attachment == null) return false;

                string filePath;
                if (ConfigurationManager.GetUseDrm())
                {
                    filePath = DrmDecryptionManager.GetDecryptFilePath(attachment.FilePath, Path.GetExtension(attachment.OriginalFileName));
                } 
                else
                {
                    filePath = attachment.FilePath;
                }

                var doc = new Document(filePath);
                var options = new PdfSaveOptions();
                options.UseHighQualityRendering = true;
                options.OutlineOptions.DefaultBookmarksOutlineLevel = 1;
                options.PageMode = PdfPageMode.UseNone;
                var targetPath = Path.Combine(DefaultPath.AttachmentPath, Guid.NewGuid().ToString());
                doc.Save(targetPath, options);

                var fileInfo = new FileInfo(targetPath);
                var size = fileInfo.Length;
                var orgPdfFileName = attachment.OriginalFileName.Substring(0, attachment.OriginalFileName.Length - 5) + ".pdf";
                var checksum = FileHelper.GetChecksum(fileInfo);

                var newAttachment = new Attachment(EFileCategory.Publication, orgPdfFileName, targetPath, size, orgPdfFileName, checksum, model.Creator, DateTime.Now);
                if (newAttachment == null) return false;

                // Create 호환
                newAttachment.TargetData = model;
                newAttachment.TargetProjectId = model.ProjectId;
                dbContext.Attachments.Add(newAttachment);
                if (saveChanges) dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}

