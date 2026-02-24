using TrustAutomation.Application.Interfaces;

namespace TrustAutomation.Infrastructure.Security
{
    public sealed class SystemClock : ISystemClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
