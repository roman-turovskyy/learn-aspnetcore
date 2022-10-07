using Example.Common;
using System;

namespace Example.Application.Tests.Fakes;

internal class FakeDateTimeProvider : IDateTimeProvider
{
    private DateTime _utcNow;
    public FakeDateTimeProvider(DateTime? utcNow = null)
    {
        _utcNow  = utcNow ?? new DateTime(2022, 10, 7, 21, 59, 47); // some arbitrary DateTime if not explicitly provided
    }

    public DateTime UtcNow => _utcNow;
}
