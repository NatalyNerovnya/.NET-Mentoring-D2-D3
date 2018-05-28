using MethodLogger;
using PostSharp.Aspects;
using System;
using System.Linq;

namespace ScanerService.Helpers
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
            _customLogger.LogBeforeCall(args.Method, args.Arguments.ToArray());
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            _customLogger.LogAfterCall(args.Method.Name, args.ReturnValue);
        }
    }
}
