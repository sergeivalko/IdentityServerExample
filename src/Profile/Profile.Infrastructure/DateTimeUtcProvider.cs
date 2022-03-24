using System;
using Profile.Application.Interfaces;

namespace Profile.Infrastructure
{
    public class DateTimeUtcProvider : IDateProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}