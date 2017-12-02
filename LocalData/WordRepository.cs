using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LocalData
{
    public class WordRepository : IRepository<WordDB>
    {
        private readonly DatabaseContext _databaseContext;

        public WordRepository(string dbPath)
        {
            _databaseContext = new DatabaseContext(dbPath);
        }

        public bool AddItem(WordDB word)
        {
            try
            {
                var tracking = _databaseContext.Words.Add(word);

                var res = _databaseContext.SaveChanges();

                var isAdded = tracking.State == EntityState.Added;

                return isAdded;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<WordDB> ContaintsInWord(string word)
        {
            try
            {
                return _databaseContext.Words.Where(o => o.Word.Contains(word)).ToList();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<WordDB> GetItems()
        {
            try
            {
                return _databaseContext.Words.ToList();
            }
            catch
            {
                return null;
            }
        }

        public IList<int> GetMatchingIds(IList<int> ids)
        {
            try
            {
                return _databaseContext.Words.Where(o => ids.Any(i => i == o.OnlineId)).Select(o => o.OnlineId).ToList();
            }
            catch
            {
                return new List<int>();
            }
        }



        public bool RemoveItem(int id)
        {
            try
            {
                var word = _databaseContext.Words.Find(id);

                var tracking = _databaseContext.Remove(word);

                _databaseContext.SaveChanges();

                var isDeleted = tracking.State == EntityState.Deleted;

                return isDeleted;
            }
            catch
            {
                return false;
            }
        }

    }
}
