using System;
using System.Threading.Tasks;

namespace Eventual.EventStore.EntityFrameworkCore
{
    public interface IUnitOfWork
    {
        Task CommitChangesAsync();
    }
}
