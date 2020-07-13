﻿namespace VOD.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class Genre : DateClass
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        //public virtual ICollection<VideoGenre> VideoLinks { get; set; }
    }
}
