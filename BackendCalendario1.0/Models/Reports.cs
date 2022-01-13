using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackendCalendario1._0.Models
{
    public class Reports
    {
        [Key]
        public DateTime date { get; set; }
        public int id_user { get; set; }
        public bool confirmed_assist { get; set; }
    }
}
