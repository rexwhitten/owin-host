using System;

namespace apistation.owin.Depends
{
    /// <summary>
    /// Channel
    /// </summary>
    public interface IChannel
    {
        void RegisterHandler<TEvent>(Type typeSelector, Action<TEvent> handler);

        void Emit<TEvent>(Type typeSelector, TEvent argument);
    }
}