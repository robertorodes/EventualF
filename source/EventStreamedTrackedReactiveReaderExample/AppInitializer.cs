using Eventual.EventStore.Readers.Tracking.Storage.EntityFramework;
using Eventual.EventStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventStreamedTrackedReactiveReaderExample
{
    static class AppInitializer
    {
        static public void Initialize()
        {
            var options = new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStreamedTrackedReactiveReaderExample;Integrated Security=False")
                .Options;

            using (var dbContext = new EventStoreDbContext(options))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                DatabaseSeeder.Seed(dbContext);
            }

            using (var dbContext = new EventStreamTrackerDbContext(
                new DbContextOptionsBuilder<EventStreamTrackerDbContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStreamedTrackedReactiveReaderExampleTrackingDb;Integrated Security=False")
                .Options))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
