using System;

namespace Hub.Shell.Domain.Providers
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}