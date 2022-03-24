using Profile.Application.Interfaces;
using StormShop.Infrastructure.Mongo;

namespace Profile.Infrastructure.Database
{
    public class ProfileRepository : BaseMongoRepository<Domain.Profile>, IProfileRepository
    {
        public ProfileRepository(IMongoContext context) : base(context)
        {
        }
    }
}