using Eventual.EventStore.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.EntityFrameworkCore
{
    public interface IEventStoreDbContext : IUnitOfWork
    {
        DbSet<Revision> StoredRevisions { get; }
    }
}
