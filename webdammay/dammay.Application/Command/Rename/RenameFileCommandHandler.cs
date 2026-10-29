using MediatR;
using Microsoft.EntityFrameworkCore;
using webdammay.dammay.Application.Command.Interfaces;

namespace webdammay.dammay.Application.Command.Rename
{
    public class RenameFileCommandHandler: IRequestHandler<RenameFileCommand,Unit>
    {
        private readonly IApplicationDbContext _context;
        public RenameFileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(RenameFileCommand request, CancellationToken cancellationToken)
        {
            var file = await _context.StoredFiles.FirstOrDefaultAsync(f => f.Id == request.FileId&&f.UserId==request.userId,cancellationToken);
            if (file == null)
                throw  new KeyNotFoundException("Không tìm thấy file hoặc bạn không có quyền đổi tên file này.");
            file.UpdateFileName(request.NewFileName);
           await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
