using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webdammay.dammay.Application.Command.Interfaces;
using webdammay.dammay.Domain.Entities;

namespace webdammay.dammay.Application.Command.Auth;

public record RegisterCommand(string Username, string Email, string Password) : IRequest<Guid>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        bool emailExists = await _context.Users.AnyAsync(x => x.Gmail == request.Email, cancellationToken);
        if (emailExists) throw new Exception("Gmail này đã được sử dụng");

        string passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = new User(request.Username, request.Email, passwordHash);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}