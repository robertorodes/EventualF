using Eventual.EventStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventStreamReactiveReaderExample
{
    static class AppInitializer
    {
        static public void Initialize()
        {
            var options = new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStreamReactiveReaderExample;Integrated Security=False")
                .Options;

            using (var dbContext = new EventStoreDbContext(options))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                DatabaseSeeder.Seed(dbContext);
            }
        }
    }
}
