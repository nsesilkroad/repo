// <copyright file="IComplianceManagementDbContext.cs" company="NSE">
// COPYRIGHT © 2014 NSE Inc. ALL RIGHTS RESERVED.
// </copyright>
// <author>wrb</author>
// <date>2018-12-21</date>

using Microsoft.EntityFrameworkCore;
using Silkroad.Core.Base.Model;

namespace Silkroad.Modules.ComplianceManagement.Model
{
    /// <summary>
    ///     Compliance 데이터 관리를 위한 DbContext 인터페이스
    /// </summary>
    public interface IComplianceManagementDbContext : IBaseDbContext
    {
        /// <summary>
        ///     Compliance 데이터 관리 DbSet
        /// </summary>
        DbSet<Compliance> Compliances { get; }
        
        /// <summary>
        ///     ComplianceTemplate 데이터 관리 DbSet
        /// </summary>
        DbSet<ComplianceTemplate> ComplianceTemplates { get; }
    }
}