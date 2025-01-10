namespace BlazorCrudApp.Shared;

public class ApiResponse<T>
{
	public bool Success { get; set; }
	public T Data { get; set; } = default!;
	public string ErrorMessage { get; set; } = string.Empty;

	public static ApiResponse<T> SuccessResponse(T data)
		=> new ApiResponse<T> { Success = true, Data = data };

	public static ApiResponse<string> ErrorResponse(string errorMessage)
		=> new ApiResponse<string> { ErrorMessage = errorMessage };
}

public class ApiResponse
{
	public bool Success { get; set; }
	public dynamic? Data { get; set; }
	public string ErrorMessage { get; set; } = string.Empty;

	public static ApiResponse SuccessResponse(dynamic? data = null, string? errorMessage = null)
		=> new ApiResponse { Success = true, Data = data, ErrorMessage = errorMessage! };

	public static ApiResponse ErrorResponse(string errorMessage)
		=> new ApiResponse { ErrorMessage = errorMessage };
}
