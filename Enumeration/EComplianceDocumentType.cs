using Silkroad.Core.Base.Attribute;

namespace Silkroad.Modules.ComplianceManagement.Enumeration
{
    /// <summary>
    ///     Compliance 등록문서 타입
    /// </summary>
    public enum EComplianceDocumentType
    {
        [EnumStringValue("pdf")] Pdf = 0,
        [EnumStringValue("docx")] Docx
    }

    /// <summary>
    ///     Compliance 등록문서 종류
    /// </summary>
    public enum EComplianceDocumentSize
    {
        [EnumStringValue("requirement")] Req = 0,
        [EnumStringValue("design")] Design
    }
}
