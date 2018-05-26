using System.Reflection;

namespace Logging
{
    public interface ICustomLogger
    {
        string LogBeforeCall(MethodBase method, object[] arguments);
        string LogAfterCall(object returnValue);
    }
}
