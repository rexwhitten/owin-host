using System;

namespace apistation.owin.Depends
{
    public interface ILog
    {
        void Log(object data);

        void Log(Exception exception);
    }
}