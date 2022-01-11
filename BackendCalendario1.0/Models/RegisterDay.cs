using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackendCalendario1._0.Models
{
    public class RegisterDay
    {
        [Key]
        public int id_register_day { get; set; }
        public int id_user { get; set; }
        public DateTime date { get; set; }
        public bool confirmed_assist { get; set; }
    }
}
