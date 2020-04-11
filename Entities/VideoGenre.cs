using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class VideoGenre
    {
        public int VideoId { get; set; }

        public virtual Video Video { get; set; }

        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }
    }
}
