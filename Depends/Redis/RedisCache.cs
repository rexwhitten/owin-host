using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apistation.owin.Models;
using System.Collections.ObjectModel;
using System.Collections;

namespace apistation.owin.Depends
{
    
    /// <summary>
    /// Redis Cache Implementation
    /// </summary>
    public class RedisCache : ICache
    {
        private static IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        private IDatabase _db;

        #region Constructors
        public RedisCache()
        {
            _db = redis.GetDatabase();
        }

        public bool HashDelete(string uri, string field)
        {
            return _db.HashDelete(uri, field);
        }

        public bool HashExists(string uri, string field)
        {
            return _db.HashExists(uri, field);
        }

        public Hashtable HashGet(string uri, string field)
        {
            var tbl = new Hashtable();
            tbl.Add(string.Format("{0}:{1}", uri, field), _db.HashGet(uri, field));
            return tbl;
        }

        public bool HashSet(string uri, EntryModel[] hashEntry)
        {
            HashEntry[] entries = convertEntries(hashEntry);
            _db.HashSet(uri, entries);
            return true;
        }

        private HashEntry[] convertEntries(EntryModel[] hashEntry)
        {
            return hashEntry.Select(h => new HashEntry(h.Field, h.Value)).ToArray();
        }
        #endregion
    }
}
