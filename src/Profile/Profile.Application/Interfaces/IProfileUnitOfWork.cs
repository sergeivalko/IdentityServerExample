using StormShop.Core.Db;

namespace Profile.Application.Interfaces
{
    public interface IProfileUnitOfWork : IUnitOfWork
    {
        public IProfileRepository ProfileRepository { get; }
    }
}