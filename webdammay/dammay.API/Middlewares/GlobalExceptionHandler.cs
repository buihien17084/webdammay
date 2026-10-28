using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace webdammay.dammay.API.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. Ghi log lỗi lại để phục vụ việc debug
        _logger.LogError(exception, "Đã xảy ra lỗi không xử lý: {Message}", exception.Message);

        // 2. Phân loại mã HTTP Status tương ứng với từng kiểu Exception
        var statusCode = exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,           // 404
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized, // 401
            ArgumentException => StatusCodes.Status400BadRequest,             // 400
            _ => StatusCodes.Status500InternalServerError                          // 500
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        // 3. Đóng gói thông tin lỗi thành cấu trúc JSON chuẩn đẹp trả về Client
        var response = new
        {
            StatusCode = statusCode,
            Message = exception.Message,
            ErrorType = exception.GetType().Name
        };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        // Trả về true để báo hiệu rằng hệ thống đã tự xử lý xong ngoại lệ này
        return true;
    }
}