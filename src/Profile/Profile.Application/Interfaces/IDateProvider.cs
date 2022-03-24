using System;

namespace Profile.Application.Interfaces
{
    public interface IDateProvider
    {
        DateTime Now { get; }
    }
}