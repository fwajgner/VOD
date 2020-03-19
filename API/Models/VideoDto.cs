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
        public string Title { get; set; }

        [Required]
        public string AltTitle { get; set; }

        [MinLength(1)]
        [MaxLength(5)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} must be numeric")]
        public string Duration { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public DateTime? ReleaseYear { get; set; }

        public List<GenreDto> Genres { get; set; }

        [Required]
        public TypeDto Type { get; set; }
    }
}
