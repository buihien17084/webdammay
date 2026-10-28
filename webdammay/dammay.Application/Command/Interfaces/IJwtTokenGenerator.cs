using webdammay.dammay.Domain.Entities;
namespace webdammay.dammay.Application.Command.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
