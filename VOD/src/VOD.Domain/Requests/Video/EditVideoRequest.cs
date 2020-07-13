namespace VOD.Domain.Requests.Video
{
    using System;

    public class EditVideoRequest
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string AltTitle { get; set; }

        public DateTime? ReleaseYear { get; set; }

        public int? Duration { get; set; }

        public string Description { get; set; }

        public bool IsInactive { get; set; }

        public int? Season { get; set; }

        public int? Episode { get; set; }

        public Guid KindId { get; set; }

        public Guid GenreId { get; set; }
    }
}
