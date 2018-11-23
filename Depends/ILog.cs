using System;

namespace apistation.owin.Depends
{
    public interface ILog
    {
        void Log(Exception exception);

        void Log(string message);
    }
}