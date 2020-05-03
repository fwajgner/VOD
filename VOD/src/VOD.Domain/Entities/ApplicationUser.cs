namespace VOD.Domain.Entities
{
    using System;

    public class ApplicationUser : DateClass
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime? SubStartDate { get; set; }

        public DateTime? SubEndDate { get; set; }
    }
}
