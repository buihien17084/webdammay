using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using webdammay.dammay.Application.Command.Interfaces;
using webdammay.dammay.Application.Command.UploadFile;
using webdammay.dammay.Domain.Entities;
using webdammay.dammay.Infrastructure.Persistence;
using Moq;
using Xunit;

namespace webdammay.Tests.Filestest.Command
{
    public class UploadFileCommandHandlerTests
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
        public async Task Handle_ShouldThrowInvalidOperationException_WhenQuotaExceeded()
        {
            // Arrange
            var context = await GetDatabaseContextAsync();
            string userId = "user-quota-test";

            // Giả lập user đã sử dụng gần hết 50MB (ví dụ: đã dùng 49MB)
            long existingSize = 499L * 1024 * 1024;
            context.StoredFiles.Add(new StoredFile("bigfile.zip", "/path/big", existingSize, userId));
            await context.SaveChangesAsync();

            var mockStorage = new Mock<IFileStorageService>();

            // Giả lập file mới upload có dung lượng 3MB (tổng sẽ vượt quá 50MB)
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(3L * 1024 * 1024);
            mockFile.Setup(f => f.FileName).Returns("exceed.mp4");

            var handler = new UploadFileCommandHandler(mockStorage.Object, context);
            var command = new UploadFileCommand(mockFile.Object, userId);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.Handle(command, CancellationToken.None)
            );

            Assert.Contains("vượt quá hạn mức dung lượng", exception.Message);
        }
    }
}
