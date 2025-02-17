namespace notebodia_api.Response
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<T> Create(T data)
        {
            return new ApiResponse<T> { Data = data };
        }

        public static ApiResponse<T> Create(T data, string message)
        {
            return new ApiResponse<T> { Data = data, Message = message };
        }
    }
}