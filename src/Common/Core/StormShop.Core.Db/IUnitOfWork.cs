using System;
using System.Threading.Tasks;

namespace StormShop.Core.Db
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}
