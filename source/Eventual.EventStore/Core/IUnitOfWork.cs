using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.EventStore.Core
{
    public interface IUnitOfWork
    {
        Task CommitChangesAsync();
    }
}
