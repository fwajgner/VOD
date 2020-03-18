namespace API.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsConfirmed { get; set; }

        public bool HaveSubscription { get; set; }

        public DateTime SubStartDate { get; set; }

        public DateTime SubEndDate { get; set; }
    }
}
