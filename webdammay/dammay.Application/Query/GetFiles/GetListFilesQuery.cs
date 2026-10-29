using MediatR;
using webdammay.dammay.Domain.Entities;

namespace webdammay.dammay.Application.Query.GetFiles;

public record GetListFilesQuery(string Id, string? SearchTerm, int PageNumber, int PageSize) : IRequest<PaginatedListDto<FileListDto>>;
public record FileListDto (Guid Id, string FileName, long FileSize, DateTime CreatedAt);


    public class PaginatedListDto<T>
    {
        public List<T> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedListDto(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }




