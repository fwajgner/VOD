namespace VOD.Domain.Entities
{
    using System;

    public class DateClass
    {
        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset? ModificationDate { get; set; }
    }
}
