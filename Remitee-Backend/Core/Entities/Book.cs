using Remitee_Backend.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Remitee_Backend.Core.Entities
{
    public class Book : Entity
    {       
        [Required]
        [MinLength(2)]
        [MaxLength(150)]
        public required string Author { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        [MaxLength(1000)]
        [Required]
        public required string Description { get; set; }
    }
}
