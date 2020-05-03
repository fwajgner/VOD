namespace VOD.Domain.Responses
{
    using System;

    public class VideoResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string AltTitle { get; set; }

        public int? Duration { get; set; }

        public string Description { get; set; }

        public DateTime? ReleaseYear { get; set; }

        public int? Season { get; set; }

        public int? Episode { get; set; }

        public Guid GenreId { get; set; }

        public GenreResponse Genre { get; set; }

        public Guid KindId { get; set; }

        public KindResponse Kind { get; set; }
    }
}
