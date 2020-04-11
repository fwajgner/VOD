namespace Entities
{
    using System;
    using System.Collections.Generic;

    public class Type : BaseClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Video> Videos { get; set; }
    }
}
