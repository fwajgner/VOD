namespace VOD.Domain.Entities
{
    using Microsoft.AspNetCore.Identity;
    using System;

    public class User : IdentityUser
    {
        public string Name { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset? ModificationDate { get; set; }

        public DateTimeOffset? SubStartDate { get; set; }

        public DateTimeOffset? SubEndDate { get; set; }
    }
}
