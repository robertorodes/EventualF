using Eventual.EventStore.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.EventStore.EntityFrameworkCore
{
    public class EventStoreDbContext : DbContext, IEventStoreDbContext
    {
        #region Properties

        public DbSet<Revision> StoredRevisions { get; set; }

        #endregion

        #region Constructors

        public EventStoreDbContext()
            : base()
        { }

        public EventStoreDbContext(DbContextOptions options)
            : base(options)
        { }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Revision>().HasKey(t => new { t.AggregateId, t.RevisionId });

            //TODO: Verify that this is the proper way (instead of configuring the property as a UNIQUE index)
            modelBuilder.Entity<Revision>().HasAlternateKey(t => t.CommitId);

            //TOOD: Review this approach for generating commit identifiers
            modelBuilder.Entity<Revision>().Property(t => t.CommitId).ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }

        public async Task CommitChangesAsync()
        {
            await this.SaveChangesAsync();
        }

        #endregion
    }
}
