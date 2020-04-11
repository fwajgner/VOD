using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class ApplicationUser : BaseClass
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime? SubStartDate { get; set; }

        public DateTime? SubEndDate { get; set; }
    }
}
