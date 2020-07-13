namespace VOD.Domain.Entities
{
    using System;

    public class VideoGenre
    {
        public Guid VideoId { get; set; }

        public virtual Video Video { get; set; }

        public Guid GenreId { get; set; }

        public virtual Genre Genre { get; set; }
    }
}
