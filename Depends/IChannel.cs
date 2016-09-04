using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
