using System.Reflection;

namespace MethodLogger
{
    public interface ICustomLogger
    {
        void LogBeforeCall(MethodBase method, object[] arguments);

        void LogAfterCall(string methodName, object returnValue);
    }
}
