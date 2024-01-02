// <copyright file="ComplianceTemplate.cs" company="NSE">
// COPYRIGHT © 2014 NSE Inc. ALL RIGHTS RESERVED.
// </copyright>
// <author>wrb</author>
// <date>2018-12-21</date>

using Silkroad.Core.Base.Enumeration.ExtTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silkroad.Modules.ComplianceManagement.Model
{
    /// <summary>
    ///     Compliance Template 정보를 관리하는 클래스
    /// </summary>
    public class ComplianceTemplate
    {
        /// <summary>
        ///     시스템 아이디
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        ///     Compliance Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Compliance Type
        /// </summary>
        public EComplianceCategoryType ComplianceCategory { get; set; }

        /// <summary>
        ///     파일명
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     태그명 - SVG 화면과 싱크를 맞추기 위해 사용
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        ///     페이즈 정보
        /// </summary>
        public virtual ComplianceTemplate ParentComplianceTemplate { get; set; }

        /// <summary>
        ///     자식 Compliance 문서 정보
        /// </summary>
        public virtual ICollection<ComplianceTemplate> ComplianceTemplateList { get; set; } = new List<ComplianceTemplate>();

        /// <summary>
        ///     순서 정보
        /// </summary>
        public int Index { get; set; }
    }
}