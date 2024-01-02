using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Silkroad.Core.Base.Enumeration.ExtTypes;
using Silkroad.Core.Base.Manager;
using Silkroad.Modules.ComplianceManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Silkroad.Modules.ComplianceManagement.Manager
{
    /// <summary>
    ///     Compliance 정보를 관리하는 클래스
    /// </summary>
    public static class ComplianceTemplateManager
    {
        private static readonly ILogger Logger = LoggerManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     선택한 카테고리의 Compliance Template 를 조회하여 반환
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="type">가져올 Compliance Template 종류</param>
        /// <returns>Compliance Template 문서 목록</returns>
        public static List<ComplianceTemplate> GetComplianceTemplate(
            this IComplianceManagementDbContext dbContext, EComplianceCategoryType type)
        {
            try
            {
                // phase와 그 자식 문서목록으로 구성됨
                var result = dbContext.ComplianceTemplates
                    .Include(row => row.ComplianceTemplateList)
                    .Where(row => row.ComplianceCategory == type && row.ParentComplianceTemplate == null)
                    .OrderBy(row => row.Index)
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
