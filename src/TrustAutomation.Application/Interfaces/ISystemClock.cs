namespace TrustAutomation.Application.Interfaces
{
    public interface ISystemClock
    {
        DateTime UtcNow { get; }
    }
}
