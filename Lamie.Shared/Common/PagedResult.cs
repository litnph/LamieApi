namespace Lamie.Shared.Common;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize)
{
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNext => Page < TotalPages;
    public bool HasPrevious => Page > 1;

    public static PagedResult<T> Empty(int page, int pageSize) =>
        new(Array.Empty<T>(), 0, page, pageSize);
}
