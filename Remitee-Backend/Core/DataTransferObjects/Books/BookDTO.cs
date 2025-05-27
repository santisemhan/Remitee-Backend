using Remitee_Backend.Core.Entities;
using Remitee_Backend.Core.Models;

namespace Remitee_Backend.Core.DataTransferObjects
{
    public class BookDTO : DataTransferObject
    {
        public Guid Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public BookDTO() { }

        public BookDTO(Book entity)
        {
            Id = entity.Id;
            Author = entity.Author;
            Title = entity.Title;
            Description = entity.Description;
        }
    }
}
