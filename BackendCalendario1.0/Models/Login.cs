using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackendCalendario1._0.Models
{
    public class Login
    {

        [Key]
        public string email { get; set; }
        public string pass { get; set; }
    }
}
