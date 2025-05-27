using Remitee_Backend.Core.Models;

namespace Remitee_Backend.Core.DataTransferObjects.Books
{
    public class GetPagedBooksDTO : DataTransferObject
    {
        public string? Author { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
    }
}
