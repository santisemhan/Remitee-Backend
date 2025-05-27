using Remitee_Backend.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Remitee_Backend.Core.DataTransferObjects.Books
{
    public class UpdateBookDTO : DataTransferObject
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 200 characters")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(150, ErrorMessage = "Author name cannot exceed 150 characters")]
        public required string Author { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public required string Description { get; set; }
    }
}
