namespace MupenToolkitPRE.Movie.Definitions
{
    public class InteractionStatus
    {
        public readonly bool Success;
        public readonly string? FailReason;
        public readonly object? CustomData;

        public InteractionStatus(bool success, string? failReason = null, object? customData = null)
        {
            Success = success;
            FailReason = failReason;
            CustomData = customData;
        }
    }
}
