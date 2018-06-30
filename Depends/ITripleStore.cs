using System.Collections.Generic;
using System.Threading.Tasks;

namespace apistation.owin.Depends
{
    public interface ITripleStore
    {
        Task AddTriple(string subjectUri, string predicateUri, string objectUri);

        Task<IEnumerable<string>> BySubject(string subjectUri); // Get Items by unique subject

        Task<IEnumerable<string>> ByPredicate(string predicateUri); // Get Items by unique predicate

        Task<IEnumerable<string>> ByObject(string objectUri); // Get Items by unique object;
    }
}