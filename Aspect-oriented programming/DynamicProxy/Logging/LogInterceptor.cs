using Castle.DynamicProxy;
using NLog;
using System.Reflection;
using System.Web.Script.Serialization;

namespace Logging
{
    public class LogInterceptor : IInterceptor
    {

        private readonly Logger _logger;

        public LogInterceptor()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Intercept(IInvocation invocation)
        {
            LogBeforeCall(invocation.Method, invocation.Arguments);
            invocation.Proceed();
            LogAfterCall(invocation.ReturnValue);
        }

        private void LogBeforeCall(MethodBase method, object[] arguments)
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
