using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apistation.owin.Depends
{
    /// <summary>
    /// Redis Channel Implementation
    /// </summary>
    public class RedisChannel : IChannel
    {
        static IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        private ISubscriber _sub = redis.GetSubscriber();

        #region constructor
        public RedisChannel()
        {

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="selector"></param>
        /// <param name="argument"></param>
        public void Emit<TEvent>(Type typeSelector, TEvent argument)
        {
            _sub.Publish(transform(typeSelector), JsonConvert.SerializeObject(argument));
        }

        private StackExchange.Redis.RedisChannel transform(Type typeSelector)
        {
            return new StackExchange.Redis.RedisChannel(typeSelector.ToTypeIdentifier(), StackExchange.Redis.RedisChannel.PatternMode.Literal);
        }

        /// <summary>
        /// Register a Channel Event Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="selector"></param>
        /// <param name="handler"></param>
        public void RegisterHandler<TEvent>(Type typeSelector, Action<TEvent> handler)
        {
            _sub.Subscribe(transform(typeSelector), (c, v) =>
            {
                TEvent args = JsonConvert.DeserializeObject<TEvent>(v.ToString());
                handler.Invoke(args);
            });
        }
    }
}
