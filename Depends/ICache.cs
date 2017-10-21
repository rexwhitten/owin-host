using apistation.owin.Models;
using System.Collections;

namespace apistation.owin.Depends
{
    /// <summary>
    /// Cache (apistation core)
    /// </summary>
    public interface ICache
    {
        bool HashExists(string uri, string field);

        Hashtable HashGet(string uri, string field);

        bool HashSet(string uri, EntryModel[] hashEntry);

        bool HashDelete(string uri, string field);
    }
}