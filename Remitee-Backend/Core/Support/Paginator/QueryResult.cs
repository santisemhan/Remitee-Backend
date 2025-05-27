namespace Remitee_Backend.Core.Support.Paginator
{
    public class QueryResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public int TotalItems { get; set; } = 0;

        public QueryResult(List<T> items, int totalItems) 
        {
            Items = items;
            TotalItems = totalItems;
        }
    }
}
