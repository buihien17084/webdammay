using MediatR;
using Microsoft.EntityFrameworkCore;
using webdammay.dammay.Application.Command.Interfaces;

namespace webdammay.dammay.Application.Command.ShareFile
{
    public class ShareFileCommandHandler: IRequestHandler<ShareFileCommand,string>
    {
        private readonly IApplicationDbContext _context;
        public ShareFileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<string> Handle(ShareFileCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Handler(ShareFileCommand request,CancellationToken cancellationToken)
        {
            var file=await _context.StoredFiles.FirstOrDefaultAsync(f=>f.Id==request.id&&f.UserId==request.userId);
            if(file==null) 
                throw new KeyNotFoundException("Không tìm thấy file hoặc bạn không có quyền chia sẻ file này.");
            var token = file.EnableSharing();
            await _context.SaveChangesAsync(cancellationToken);
            return token;
        }
    }
}
