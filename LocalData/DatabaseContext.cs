using System;
using Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LocalData
{

    public class DatabaseContext : DbContext
    {
        public DbSet<WordDB> Words { get; set; }

        private readonly string _databasePath;

        public DatabaseContext(string databasePath)
        {
            _databasePath = databasePath;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(string.Format("Filename={0}", _databasePath));
        }
    }
}
