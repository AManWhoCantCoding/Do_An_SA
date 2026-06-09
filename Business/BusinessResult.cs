namespace MediSphere.Business
{
    public class BusinessResult<T>
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public string? ErrorMessage { get; init; }

        public static BusinessResult<T> Ok(T data) => new() { Success = true, Data = data };

        public static BusinessResult<T> Fail(string message) => new() { Success = false, ErrorMessage = message };
    }
}
