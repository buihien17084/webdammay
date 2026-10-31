using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using webdammay.dammay.Application.Query.GetFiles;
using webdammay.dammay.Domain.Entities;
using webdammay.dammay.Infrastructure.Persistence;
using Xunit;
namespace webdammay.Tests.Filestest.Command
{
    public class GetFilesQueryHandlerTests
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
        public async Task Handle_ShouldFilterBySearchTermAndPaginateCorrectly()
        {
            // Arrange
            var context = await GetDatabaseContextAsync();
            string userId = "user-search-test";

            // Thêm dữ liệu mẫu vào InMemory DB
            context.StoredFiles.Add(new StoredFile("report_2025.pdf", "/p/1", 100, userId));
            context.StoredFiles.Add(new StoredFile("report_2026.pdf", "/p/2", 200, userId));
            context.StoredFiles.Add(new StoredFile("image_avatar.png", "/p/3", 300, userId));
            await context.SaveChangesAsync();

            var handler = new GetListFilesQueryHandler(context);

            // Truy vấn tìm các file có chứa từ khóa "report" với PageSize = 1
            var query = new GetListFilesQuery(userId, SearchTerm: "report", PageNumber: 1, PageSize: 1);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount); // Tổng số file khớp từ khóa "report" là 2
            Assert.Single(result.Items);          // Do PageSize = 1 nên trang 1 chỉ trả về 1 item
            Assert.Equal(2, result.TotalPages);   // Tổng số trang phải là 2
            Assert.True(result.HasNextPage);      // Phải có trang tiếp theo
        }
    }
}
