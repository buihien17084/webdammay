using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webdammay.dammay.Application.Query.GetFiles;
using webdammay.dammay.Application.Command.UploadFile;
using Microsoft.AspNetCore.Authorization;
namespace webdammay.dammay.API.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/files")]
    public class Filecontroller : ControllerBase
    {
        private readonly IMediator _mediator;
        public Filecontroller(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { Message = "File không hợp lệ hoặc rỗng." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Không tìm thấy danh tính người dùng." });

            var command = new UploadFileCommand(file, userId);
            var result = await _mediator.Send(command);

            return Ok(new { Message = "Upload file thành công!", Data = result });
        }
        [HttpGet("GetMyFiles")]
        public async Task<IActionResult> GetMyFiles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Không tìm thấy người dùng" });
            var query = new GetFilesQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(new { Message = "Lấy danh sách Files thành công", Data = result });
        }
        [HttpGet("{id}/download")]
        public async Task<IActionResult> Downlaod( Guid Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Không tìm thấy người dùng" });
            var query = new GetFileQuery(Id, userId);
            var result= await _mediator.Send(query);
            return File(result.FileStream,result.ContentType, result.FileName);
         }

    }
}
