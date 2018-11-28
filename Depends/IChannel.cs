using System;

namespace apistation.owin.Depends
{
    /// <summary>
    /// Channel
    /// </summary>
    public interface IChannel
    {
        void RegisterHandler<TEvent>(Action<TEvent> handler);

        void Emit<TEvent>(TEvent argument);
    }
}