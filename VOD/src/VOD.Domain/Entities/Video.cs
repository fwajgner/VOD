namespace VOD.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class Video : DateClass
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string AltTitle { get; set; }

        public DateTime? ReleaseYear { get; set; }

        public ushort? Duration { get; set; }

        public string Description { get; set; }

        public bool IsInactive { get; set; }

        public ushort? Season { get; set; }

        public ushort? Episode { get; set; }

        public Guid KindId { get; set; }

        public virtual Kind Kind { get; set; }

        public Guid GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        //public virtual ICollection<VideoGenre> GenreLinks { get; set; }
    }
}
