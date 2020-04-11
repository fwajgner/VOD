using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class VideoDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(150)]
        public string AltTitle { get; set; }

        [MinLength(1)]
        [MaxLength(5)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} must be positive number")]
        public string Duration { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime? ReleaseYear { get; set; }

        public List<GenreDto> Genres { get; set; }

        [Required]
        public string TypeName { get; set; }
    }
}
