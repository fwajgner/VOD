using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class SeriesDto : VideoDto
    {
        [MinLength(1)]
        [MaxLength(5)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} must be numeric")]
        public string Season { get; set; }

        [MinLength(1)]
        [MaxLength(5)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} must be numeric")]
        public string Episode { get; set; }
    }
}
