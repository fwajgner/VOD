using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Genre : BaseClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<VideoGenre> VideoLinks { get; set; }
    }
}
