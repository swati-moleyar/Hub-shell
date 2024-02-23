namespace Hub.Shell.Error
{
    public class ServiceError
    {
        public string Message { get; set; }
        public ErrorType Type { get; set; } = ErrorType.Unknown;

        public ServiceError(ErrorType type, string message)
        {
            Type = type;
            Message = message;
        }

        public ServiceError(ErrorType type)
        {
            Type = type;
        }

        public ServiceError(string message)
        {
            Message = message;
        }

        public ServiceError() { }
    }

    public enum ErrorType
    {
        Unknown,
        ValidationFailure,
        BadRequestData,
        InvalidToken,
        InvalidPermission,
        NotFound,
    }
}