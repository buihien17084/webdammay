using webdammay.dammay.Application.Command.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace webdammay.dammay.Application.Command.Auth
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthenticationResult>;
    public record AuthenticationResult(Guid id, string Username, string Email, string Token);
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public LoginCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticationResult> Handler(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Gmail == request.Email, cancellationToken);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không chính xác.");
            }
            var token = _jwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult(user.Id, user.UserName, user.Gmail, token);

        }
    }
}
