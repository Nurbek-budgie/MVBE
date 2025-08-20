namespace DTO.Middleware;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    
    public static ApiResponse<T> Success(T data) => new ApiResponse<T> { Data = data, IsSuccess = true };
    public static ApiResponse<T> Failure(string message) => new ApiResponse<T> { IsSuccess = false, Message = message };
}