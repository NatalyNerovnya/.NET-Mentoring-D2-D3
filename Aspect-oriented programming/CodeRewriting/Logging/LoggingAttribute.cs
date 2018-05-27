using PostSharp.Aspects;
using System;

namespace Logging
{
    [Serializable]
    public class LoggerAttribute : OnMethodBoundaryAspect
    {
        private ICustomLogger _customLogger;

        public LoggerAttribute()
        {
            _customLogger = new CustomLogger();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            var log = _customLogger.LogBeforeCall(args.Method, args.Method.GetParameters());
            Console.WriteLine(log);
            
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var log = _customLogger.LogAfterCall(args.ReturnValue);
            Console.WriteLine(log);
        }
    }
}
