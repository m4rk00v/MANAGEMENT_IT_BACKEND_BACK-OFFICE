public static class ApiResponseFactory
{
    public static ApiResponse<T> Success<T>(T data, string message = "Successfully operation")
    {
        return new ApiResponse<T>(true, message, data);
    }

    public static ApiResponse<T> Error<T>(string message, T data = default)
    {
        return new ApiResponse<T>(false, message, data);
    }
}