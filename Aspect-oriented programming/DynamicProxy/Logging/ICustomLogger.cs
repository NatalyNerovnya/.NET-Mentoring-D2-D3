using System.Reflection;

namespace Logging
{
    public interface ICustomLogger
    {
        void LogBeforeCall(MethodBase method, object[] arguments);

        void LogAfterCall(object returnValue);
    }
}
