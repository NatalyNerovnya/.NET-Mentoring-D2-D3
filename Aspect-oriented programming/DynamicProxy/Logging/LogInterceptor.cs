using Castle.DynamicProxy;
using System;

namespace Logging
{
    public class LogInterceptor : IInterceptor
    {

        private readonly ICustomLogger _logger;

        public LogInterceptor()
        {
            _logger = new Logger();
        }

        public void Intercept(IInvocation invocation)
        {
            _logger.LogBeforeCall(invocation.Method, invocation.Arguments);
            invocation.Proceed();
            var log = _logger.LogAfterCall(invocation.ReturnValue);

            Console.WriteLine(log);
        }

    }
}
