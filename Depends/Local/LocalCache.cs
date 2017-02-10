using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apistation.owin.Models;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.Isam.Esent.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Database.Isam.Config;

namespace apistation.owin.Depends.Local
{
    public static class Extensions
    {
        public static void Each<T>(this IEnumerable<T> items, Action<T> reducer)
        {
            foreach(var i in items)
            {
                reducer.Invoke(i);
            }
        }
    }
    

    public class LocalCache : ICache
    {
        #region Fields 
        private string _storagePath;
        #endregion

        #region Constructor
        public LocalCache()
        {
            this._storagePath = @"C:\data\cacheJ";
        }
        #endregion

        public bool HashDelete(string uri, string field)
        {
            string key = string.Format("{0}::{1}", uri, field);
            return Db(db => db.Remove(key));
        }

        public bool HashExists(string uri, string field)
        {
            string key = string.Format("{0}::{1}", uri, field);
            return Db(db => db.Keys.Contains(key));
        }

        public Hashtable HashGet(string uri, string field)
        {
            return Db(db =>
            {
                var resultSet = new Hashtable();
                db.Where(i => i.Key.StartsWith(uri))
                  .Each(item => resultSet.Add(item.Key, item.Value));
                return resultSet;
            });
        }

        public bool HashSet(string uri, EntryModel[] hashEntry)
        {
            foreach(var entry in hashEntry)
            {
                string key = string.Format("{0}::{1}", uri, entry.Field);

                Db(db => {

                    if (db.ContainsKey(key))
                    {
                        db[key] = JObject.Parse(entry.Value);
                    }
                    else
                    {
                        db.Add(key, JObject.Parse(entry.Value));
                    }

                    return true;
                });
            }

            return true;
        }

        #region Private Methods
        /// <summary>
        /// Use Local Database
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="use"></param>
        /// <returns></returns>
        private TResult Db<TResult>(Func<PersistentDictionary<string, JObject>, TResult> use)
        {
            using(PersistentDictionary<string, JObject> pd = new PersistentDictionary<string, JObject>(this._storagePath))
            {
                // switch on which store to use
                return use.Invoke(pd);
            }
        }
        #endregion
    }
}
