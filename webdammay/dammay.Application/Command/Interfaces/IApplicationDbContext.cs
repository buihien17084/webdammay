using Microsoft.EntityFrameworkCore;

using webdammay.dammay.Domain.Entities;

namespace webdammay.dammay.Application.Command.Interfaces;

public interface IApplicationDbContext
{
    DbSet<StoredFile> StoredFiles { get; }
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}