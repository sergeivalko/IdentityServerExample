using System;

namespace Profile.Domain
{
    public class Profile
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Photo { get; set; }
    }
}