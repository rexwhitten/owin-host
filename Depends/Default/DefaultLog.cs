using System;

namespace apistation.owin.Depends
{
    public class DefaultLog : ILog
    {
        public DefaultLog()
        {
        }

        public void Log(Exception exception)
        {
            Console.WriteLine(string.Format("ERROR:{0}", exception.Message));
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}