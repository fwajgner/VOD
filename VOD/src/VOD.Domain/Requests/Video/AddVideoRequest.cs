namespace VOD.Domain.Requests.Video
{
    using System;

    public class AddVideoRequest
    {
        public string Title { get; set; }

        public string AltTitle { get; set; }

        public DateTime? ReleaseYear { get; set; }

        public int? Duration { get; set; }

        public string Description { get; set; }

        public int? Season { get; set; }

        public int? Episode { get; set; }

        public Guid KindId { get; set; }

        public Guid GenreId { get; set; }
    }
}
