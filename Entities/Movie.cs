using System;

namespace Model
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public short ReleaseYear { get; set; }

        public short Duration { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }
    }
}
