using System;

namespace apistation.owin.Depends
{
    using Newtonsoft.Json;

    public class DefaultLog : ILog
    {
        public DefaultLog()
        {
        }

        public void Log(Exception exception)
        {
            Console.WriteLine(string.Format("ERROR:{0}", exception.Message));
        }

        public void Log(object data)
        {
            Console.WriteLine(string.Format("{0}:{1}", DateTime.Now.ToString(), JsonConvert.SerializeObject(data)));
        }
    }
}