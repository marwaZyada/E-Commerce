namespace Talabat.Api.Helpers
{
    public class Pagination<T>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        public Pagination(int pindex,int psize,int count, IReadOnlyList<T> data)
        {
            PageIndex = pindex;
            PageSize = psize;
            Count = count;
            Data = data;
            
        }
    }
}
