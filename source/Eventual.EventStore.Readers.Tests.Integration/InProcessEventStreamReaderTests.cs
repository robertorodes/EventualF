using Eventual.EventStore.Core;
using Eventual.EventStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Eventual.EventStore.Readers.Tests.Integration
{
    public class InProcessEventStreamReaderTests: IClassFixture<InProcessEventStreamReaderTestsFixture>
    {
        [Fact]
        public async Task GetEventStreamFromAsync_WithValidArgs_DoesNotThrowException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStreamReadersIntegrationTests;Integrated Security=False")
                .Options;
            EventStoreDbContext context = new EventStoreDbContext(options);
            IEventStore eventStore = new EFEventStore(context, null);
            IEventStreamReader client = new InProcessEventStreamReader(eventStore);
            int expectedNumberOfRevisions = 7;

            // Act
            EventStream stream = await client.GetEventStreamFromAsync("19e2a7fc-d0eb-44b9-8348-e7678ccd37bc", -1, 10);

            // Assert
            Assert.Equal<int>(expectedNumberOfRevisions, stream.Revisions.Count);

            context.Dispose();
        }
    }
}
