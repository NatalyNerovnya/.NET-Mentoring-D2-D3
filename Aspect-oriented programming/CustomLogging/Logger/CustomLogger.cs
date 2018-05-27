using System;
using NLog;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Text;

namespace MethodLogger
{
    [Serializable]
    public class CustomLogger : ICustomLogger
    {
        [NonSerialized]
        private Logger _logger;

        public CustomLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void LogBeforeCall(MethodBase method, object[] args)
        {
            if (_logger == null)
            {
                _logger = LogManager.GetCurrentClassLogger();
            }

            var arguments = AppendArguments(args);
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

        private static string AppendArguments(object[] arguments)
        {
            var sb = new StringBuilder();

            sb.Append('(');

            for (var i = 0; i < arguments.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }

                sb.Append(arguments[i]);
            }

            sb.Append(')');
            return sb.ToString();
        }
    }
}
