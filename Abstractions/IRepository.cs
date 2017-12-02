using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstractions
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetItems();

        bool AddItem(T product);

        bool RemoveItem(int id);

        IList<int> GetMatchingIds(IList<int> ids);

        IEnumerable<T> ContaintsInWord(string word);
    }
}
