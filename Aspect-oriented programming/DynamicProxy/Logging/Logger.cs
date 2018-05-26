using System;
using System.Reflection;
using System.Text;

namespace Logging
{
    [Serializable]
    public class Logger : ICustomLogger
    {
        private StringBuilder _sb;

        public string LogBeforeCall(MethodBase method, object[] arguments)
        {
            _sb = new StringBuilder();

            _sb.AppendFormat("{0} : ", DateTime.Now.TimeOfDay);
            _sb.AppendFormat("Called: {0}.{1}", method.DeclaringType.FullName, method.Name);
            _sb.Append(AppendArguments(arguments));
            return _sb.ToString();
        }

        public string LogAfterCall(object returnValue)
        {
            if (returnValue == null)
            {
                return _sb.ToString();
            }

            _sb.AppendFormat(" Returned value : {0}", returnValue);
            return _sb.ToString();
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
