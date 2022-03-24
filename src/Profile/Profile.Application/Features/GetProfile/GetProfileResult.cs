using System;

namespace Profile.Application.Features.GetProfile
{
    public class GetProfileResult 
    {
        public Guid AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public DateTime Created { get; set; }
    }
}