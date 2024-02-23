using System;

namespace Hub.Shell.Domain.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow { get => DateTime.UtcNow; }
    }
}