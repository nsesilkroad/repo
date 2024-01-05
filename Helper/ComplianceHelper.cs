using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Silkroad.Core.Base.Const;
using Silkroad.Core.Base.Helper;
using Silkroad.Core.Base.Manager;
using Silkroad.Core.Base.Model;
using Silkroad.Modules.ComplianceManagement.Model;
using System;
using System.Linq;
using System.Reflection;

namespace Silkroad.Modules.ComplianceManagement.Helper
{
    public static class ComplianceHelper
    {
        /// <summary>
        ///     Logger
        /// </summary>
        private static readonly ILogger Logger = LoggerManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	
	ComplianceHelper (String name)
	{
		Name = name;	
	}
    }
}
