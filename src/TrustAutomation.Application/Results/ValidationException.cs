namespace TrustAutomation.Application.Results
{
    public sealed class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
