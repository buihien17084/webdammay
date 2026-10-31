using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using webdammay.dammay.Application.Command.DeleteFile;
using webdammay.dammay.Application.Command.Interfaces;
using webdammay.dammay.Domain.Entities;
using webdammay.dammay.Infrastructure.Persistence;

namespace webdammay.Tests.Filestest.Command
{
    public class DeleteFileCommandHandlerTests
    {
        private async Task<ApplicationDbContext> GetDatabaseContextAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenUserDoesNotOwnTheFile()
        {
            // Arrange
            var context = await GetDatabaseContextAsync();
            string ownerId = "real-owner-id";
            string hackerId = "hacker-id";

            var file = new StoredFile("secret.txt", "/uploads/secret.txt", 1024, ownerId);
            context.StoredFiles.Add(file);
            await context.SaveChangesAsync();

            var mockStorage = new Mock<IFileStorageService>();
            var handler = new DeleteFileCommandHandler(context, mockStorage.Object);

            // Hacker cố gắng xóa file không thuộc sở hữu của mình
            var command = new DeleteFileCommand(file.Id, hackerId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => handler.Handle(command, CancellationToken.None)
            );

            // Đảm bảo dịch vụ lưu trữ file vật lý KHÔNG BAO GIỜ bị gọi xóa nhầm file
            mockStorage.Verify(s => s.DeleteFileAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
