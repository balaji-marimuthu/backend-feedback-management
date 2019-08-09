using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace FeedbackApplication.Service
{
    [ExcludeFromCodeCoverage]
    public static class LogService
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static ILog Logger()
        {
            return log;
        }

    }
}