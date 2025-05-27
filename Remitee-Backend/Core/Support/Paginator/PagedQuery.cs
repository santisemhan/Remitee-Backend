using Remitee_Backend.Core.Support.Validators;
using System.ComponentModel.DataAnnotations;

namespace Remitee_Backend.Core.Support.Paginator
{
    public class PagedQuery<T> where T : class
    {
        public T? Filter { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page number not valid")]
        public int PageNumber { get; set; }

        [Range(1, 1000, ErrorMessage = "The size of the page can't be lower than 1 and bigger than 1000")]
        public int PageSize { get; set; }

        public string? SortField { get; set; }

        [StringContains(AllowableValues = new[] { "ASC", "DESC" }, IsCaseSensitive = false, Nulleable = true)]
        public string SortOrder { get; set; }

        public PagedQuery()
        {
            PageNumber = 1;
            PageSize = 10;
            SortField = null;
            SortOrder = "ASC";
            Filter = null;
        }
    }
}
