using System;
using Microsoft.EntityFrameworkCore;
using Momentary.Common;

namespace Momentary.Sqlite
{
    public class MomentaryDatabaseFixture<TContext> : IDisposable where TContext : DbContext
    {
        private readonly IDatabase _database;
        
        public TContext DatabaseContext { get; }

        public MomentaryDatabaseFixture(DatabaseBuilder builder)
        {
            _database = builder.Build();
            var contextBuilder = new DbContextOptionsBuilder<TContext>()
                .UseSqlite(builder.TransientConnectionString);
            DatabaseContext = (TContext) Activator.CreateInstance(typeof(TContext), contextBuilder);
            DatabaseContext.Database.EnsureCreated();
        }
        
        public void Dispose()
        {
            _database.Drop();
        }
    }
}