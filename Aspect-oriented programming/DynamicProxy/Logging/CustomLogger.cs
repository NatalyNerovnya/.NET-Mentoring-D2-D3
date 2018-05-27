using System.Reflection;
using System.Web.Script.Serialization;
using NLog;
using System;

namespace Logging
{
    [Serializable]
    public class CustomLogger: ICustomLogger
    {
        private Logger _logger;

        public CustomLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void LogBeforeCall(MethodBase method, object[] arguments)
        {
            var data = new { method.DeclaringType.FullName, method.Name, arguments };
            var json = new JavaScriptSerializer().Serialize(data);

            _logger.Trace(json);
        }

        public void LogAfterCall(object returnValue)
        {
            if (returnValue != null)
            {
                var json = new JavaScriptSerializer().Serialize(returnValue);
                _logger.Trace(json);
            }
        }
    }
}
