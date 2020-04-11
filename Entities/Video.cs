using System;
using System.Collections.Generic;

namespace Entities
{
    public class Video : BaseClass
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AltTitle { get; set; }

        public DateTime? ReleaseYear { get; set; }

        public ushort? Duration { get; set; }

        public string Description { get; set; }

        public ushort? Season { get; set; }

        public ushort? Episode { get; set; }

        public int TypeId { get; set; }

        public virtual Entities.Type Type { get; set; }

        public virtual ICollection<VideoGenre> GenreLinks { get; set; }
    }
}
