using Microsoft.EntityFrameworkCore;
using Silkroad.Core.Base.Data.Query;
using Silkroad.Core.Base.Helper;
using Silkroad.Core.Base.Manager;
using Silkroad.Core.Base.Model;
using Silkroad.Modules.ComplianceManagement.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Silkroad.Modules.ComplianceManagement.Manager.TrackerDataManager
{
    public class ComplianceTrackerDataManager : IComplianceTrackerDataManager
    {
        public T AddBaseTrackerData<T>(IDbContext dbContext, T targetData) where T : BaseTracker
        {
            var complianceDbContext = (IComplianceManagementDbContext)dbContext;
            var userId = UserManager.GetUserManager().GetUserId(HttpContextHelper.GetHttpContext().User);
            var member = complianceDbContext.Members.FirstOrDefault(row => row.Id == Int32.Parse(userId));
            // PDF 변환 처리
            if (targetData.Attachments.Any(attach => (
                !attach.IsDeleted &&
                Path.GetExtension(attach.OriginalFileName).Equals(".docx") &&
                attach.Category == EFileCategory.Publication
            ))) {
                complianceDbContext.CreatePdf(targetData, member, false);
            }
            complianceDbContext.Compliances.Add(targetData as Compliance);
            return targetData;
        }

        public IList<T> AddBaseTrackerData<T>(IDbContext dbContext, IList<T> targetData) where T : BaseTracker
        {
            return targetData;
        }

        public T DeleteBaseTrackerData<T>(IDbContext dbContext, T targetData, BaseMember member) where T : BaseTracker
        {
            targetData.IsDeleted = true;
            return targetData;
        }

        public IList<T> DeleteBaseTrackerData<T>(IDbContext dbContext, IList<T> targetData, BaseMember member) where T : BaseTracker
        {
            return targetData;
        }

        public T GetBaseTrackerData<T>(IDbContext dbContext, int id) where T : BaseTracker
        {
            var complianceDbContext = (IComplianceManagementDbContext)dbContext;
            var returnValue = complianceDbContext.Compliances.FirstOrDefault(compliance => compliance.Id == id);
            if(returnValue.ParentId != null)
            {
                returnValue.Leaf = true;
            }
            complianceDbContext.Attachments.Where(attach => attach.TargetDataId == targetData.Id && !attach.IsDeleted).Load();
            return returnValue as T;
        }

        public IQueryable<T> GetTrackerDataList<T>(IDbContext dbContext, Query query = null) where T : BaseTracker
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetTrackerDataList<T>(IDbContext dbContext, Query query, ref int totalCount) where T : BaseTracker
        {
            throw new NotImplementedException();
        }

        public T ReadBaseTrackerData<T>(IDbContext dbContext, T targetData, BaseMember member) where T : BaseTracker
        {
            var complianceDbContext = (IComplianceManagementDbContext)dbContext;
            complianceDbContext.Attachments.Where(attach => attach.TargetDataId == targetData.Id && !attach.IsDeleted).Load();

            return targetData;
        }

        public T UpdateBaseTrackerData<T>(IDbContext dbContext, T targetData) where T : BaseTracker
        {
            var complianceDbContext = (IComplianceManagementDbContext)dbContext;
            var userId = UserManager.GetUserManager().GetUserId(HttpContextHelper.GetHttpContext().User);
            var member = complianceDbContext.Members.FirstOrDefault(row => row.Id == Int32.Parse(userId));
            // PDF 변환 처리
            if (targetData.Attachments.Any(attach => (
                !attach.IsDeleted &&
                Path.GetExtension(attach.OriginalFileName).Equals(".docx") &&
                attach.Category == EFileCategory.Publication
            )))
            {
                complianceDbContext.CreatePdf(targetData, member, false);
            }
            return targetData;
        }

        public IList<T> UpdateBaseTrackerData<T>(IDbContext dbContext, IList<T> targetData) where T : BaseTracker
        {
            return targetData;
        }
    }
}
