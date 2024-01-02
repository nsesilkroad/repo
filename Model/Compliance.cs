// <copyright file="Compliance.cs" company="NSE">
// COPYRIGHT © 2014 NSE Inc. ALL RIGHTS RESERVED.
// </copyright>
// <author>wrb</author>
// <date>2018-12-21</date>


using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Silkroad.Core.Base.Enumeration.ExtTypes;
using Silkroad.Core.Base.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace Silkroad.Modules.ComplianceManagement.Model
{
    /// <summary>
    ///     Compliance 데이터
    /// </summary>
    public class Compliance : BaseTracker
    {
        public Compliance()
        {
        }

        /// <summary>
        ///     기본 생성자
        /// </summary>
        /// <param name="name"></param>
        public Compliance(string name) : base(name)
        {
        }

        /// <summary>
        ///     DbContext 를 가지는 생성자
        /// </summary>
        /// <param name="dbContext"></param>
        public Compliance(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        ///     실 파일명
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     Compliance Type
        /// </summary>
        public EComplianceCategoryType ComplianceCategory { get; set; }

        /// <summary>
        ///     태그명 - SVG 화면과 싱크를 맞추기 위해 사용
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        ///     정렬 순서
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Compliance의 Description 필드
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     화면에서 사용하는 컬럼
        /// </summary>
        [NotMapped]
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        ///     화면에서 사용하는 컬럼
        /// </summary>
        [NotMapped]
        [JsonProperty("expanded")]
        public bool Expanded { get; set; }

        /// <summary>
        ///     Leaf 값을 알려주는 필드
        /// </summary>
        [NotMapped]
        [JsonProperty("leaf")]
        public bool Leaf { get; set; }

        /// <summary>
        ///     UseCheckBox의 사용 여부 필드 - 기본값 false
        /// </summary>
        [NotMapped]
        [JsonProperty("checked")]
        public bool? Checked { get; set; }

        /// <summary>
        ///     문서로 등록한 파일의 PDF 여부
        /// </summary>
        [NotMapped]
        [JsonProperty("UsePdf")]
        public string UsePdf
        {
            get
            {
                return Attachments.Any(a => 
                    !a.IsDeleted && 
                    a.Category == EFileCategory.Publication && 
                    a.OriginalFileName.Contains(".pdf")) ? "1" : "0";
            }
        }
    }
}