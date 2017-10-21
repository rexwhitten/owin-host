using System;

namespace apistation.owin.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandOptionsAttribute : System.Attribute
    {
        public String Method { get; private set; }

        public string PathExpression { get; private set; }

        public CommandOptionsAttribute(string method, String pathExpression)
        {
            Method = method;
            PathExpression = pathExpression;
        }
    }
}