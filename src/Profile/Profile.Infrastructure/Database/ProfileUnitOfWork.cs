using Profile.Application.Interfaces;
using StormShop.Infrastructure.Mongo;

namespace Profile.Infrastructure.Database
{
    public sealed class ProfileUnitOfWork : BaseMongoUnitOfWork, IProfileUnitOfWork
    {
        private readonly IMongoContext _context;
        private IProfileRepository _profileRepository;

        public ProfileUnitOfWork(IMongoContext context) : base(context)
        {
            _context = context;
        }

        public IProfileRepository ProfileRepository => _profileRepository ??= new ProfileRepository(_context);
    }
}
