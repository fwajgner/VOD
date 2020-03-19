namespace API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserDetailsDto : UserDto
    {
        public Guid? Id { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsConfirmed { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ModificationDate { get; set; }
    }
}
