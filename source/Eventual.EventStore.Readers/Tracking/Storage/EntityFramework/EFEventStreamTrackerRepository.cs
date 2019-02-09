using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Eventual.EventStore.Readers.Tracking.Storage.EntityFramework
{
    public class EFEventStreamTrackerRepository : IEventStreamTrackerRepository
    {
        #region Attributes

        private EventStreamTrackerDbContext dbContext;

        #endregion

        #region Constructors

        public EFEventStreamTrackerRepository(EventStreamTrackerDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        #endregion

        #region Methods

        public async Task<EventStreamTracker> GetEventStreamTracker(string trackerId)
        {
            //var results = from tracker in this.DbContext.EventStreamTrackers
            //              where tracker.TrackerId == trackerId
            //              select tracker;

            //return await results.FirstOrDefaultAsync();

            return await GetEventStreamTracker(trackerId, false);
        }

        public async Task<EventStreamTracker> GetEventStreamTracker(string trackerId, bool refresh)
        {
            var results = from tracker in this.DbContext.EventStreamTrackers
                          where tracker.TrackerId == trackerId
                          select tracker;

            var trackerEntity = await results.FirstOrDefaultAsync();

            if (trackerEntity != null && refresh == true)
            {
                await this.DbContext.Entry(trackerEntity).ReloadAsync();
            }

            return trackerEntity;
        }

        public async Task CreateEventStreamTracker(EventStreamTracker tracker)
        {
            this.DbContext.EventStreamTrackers.Add(tracker);

            // Nothing to await in this implementation
            await Task.FromResult(true);
        }

        public async Task UpdateEventStreamTracker(EventStreamTracker tracker)
        {
            //Nothing to be done here
            await Task.FromResult(true);
        }

        public async Task CommitChanges()
        {
            try
            {
                await this.DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new EventStreamTrackedReaderDbConcurrencyException(EventStreamTrackedReaderDbConcurrencyException.DefaultMessage, ex);
            }
        }

        #endregion

        #region Properties            

        private EventStreamTrackerDbContext DbContext
        {
            get
            {
                return this.dbContext;
            }
            set
            {
                //TODO: Insert validation code here
                this.dbContext = value;
            }
        }

        #endregion
    }
}
