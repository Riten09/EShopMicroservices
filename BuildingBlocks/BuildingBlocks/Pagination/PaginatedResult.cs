namespace BuildingBlocks.Pagination
{
    public class PaginatedResult<TEnity>
        (int pageIndex, int pageSize, long count, IEnumerable<TEnity> data)
        where TEnity : class // means this TEnity should be class
    {
        public int PageIndex { get; } = pageIndex;
        public int PageSize { get; } = pageSize;
        public long Count { get; } = count;
        public IEnumerable<TEnity> Data { get; } = data;
    }
}
