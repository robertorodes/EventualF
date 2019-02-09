using Eventual.EventStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Readers.Tests.Integration
{
    public class InProcessEventStreamReaderTestsFixture : IDisposable
    {
        public InProcessEventStreamReaderTestsFixture()
        {
            var options = new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStreamReadersIntegrationTests;Integrated Security=False")
                .Options;

            using (var dbContext = new EventStoreDbContext(options))
            {
                DatabaseSeeder.Seed(dbContext);
            }
        }

        public void Dispose()
        {
        }
    }
}
