using Castle.DynamicProxy;
using MethodLogger;

namespace Scaner.Logging
{
    public class LogInterceptor : IInterceptor
    {

        private readonly ICustomLogger _logger;

        public LogInterceptor()
        {
            _logger = new CustomLogger();
        }

        public void Intercept(IInvocation invocation)
        {
            _logger.LogBeforeCall(invocation.Method, invocation.Arguments);
            invocation.Proceed();
            _logger.LogAfterCall(invocation.ReturnValue);
        }
    }
}
