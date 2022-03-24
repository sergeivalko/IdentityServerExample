using System;

namespace Profile.Domain
{
    public class Profile
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Profile(Guid id, DateTime created)
        {
            Id = id;
            Created = created;
        }
    }
}