using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Readers.Tracking.Storage.EntityFramework
{
    public class EventStreamTrackerDbContext : DbContext
    {
        #region Attributes

        public DbSet<EventStreamTracker> EventStreamTrackers { get; set; }

        #endregion

        #region Constructors

        public EventStreamTrackerDbContext() : base() { }

        public EventStreamTrackerDbContext(DbContextOptions<EventStreamTrackerDbContext> options) : base(options) { }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventStreamTracker>().HasKey(t => t.TrackerId);
            modelBuilder.Entity<EventStreamTracker>().OwnsOne(p => p.GlobalCheckpoint);
            modelBuilder.Entity<EventStreamTracker>().OwnsOne(p => p.StreamCheckpoint);

            // Concurrency configuration
            modelBuilder.Entity<EventStreamTracker>().Property(p => p.RowVersion).IsConcurrencyToken().IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
