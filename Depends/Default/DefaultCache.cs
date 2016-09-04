using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apistation.owin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace apistation.owin.Depends
{
    public class DefaultCache : ICache
    {
        #region static members
        static Dictionary<string, JObject> _cache = new Dictionary<string, JObject>();
        #endregion

        #region constructors
        public DefaultCache()
        {
               
        }
        #endregion

        public bool HashDelete(string uri, string field)
        {
           return _cache.Remove(string.Format("{0}:{1}", uri, field));
        }

        public bool HashExists(string uri, string field)
        {
            return _cache.ContainsKey(string.Format("{0}:{1}", uri, field));
        }

        public Hashtable HashGet(string uri, string field)
        {
            var tbl = new Hashtable();
            var results = _cache.Where(i => i.Key.StartsWith(uri));

            foreach(var result in results)
            {
                tbl.Add(result.Key, result.Value);
            }

            return tbl;
        }

        public bool HashSet(string uri, EntryModel[] hashEntry)
        {
            foreach(var entry in hashEntry)
            {
                if(_cache.ContainsKey(string.Format("{0}:{1}", uri, entry.Field)))
                {
                    _cache[string.Format("{0}:{1}", uri, entry.Field)] = JObject.Parse(entry.Value);
                }
                else
                {
                    _cache.Add(string.Format("{0}:{1}", uri, entry.Field), JObject.Parse(entry.Value));
                }
            }

            return true;
        }
    }
}
