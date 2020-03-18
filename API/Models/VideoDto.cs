using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class VideoDto
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string AltTitle { get; set; }

        public short? Duration { get; set; }

        public string Description { get; set; }

        public DateTime? ReleaseYear { get; set; }

        public List<GenreDto> Genres { get; set; }

        public TypeDto Type { get; set; }
    }
}
