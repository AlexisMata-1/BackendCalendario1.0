using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackendCalendario1._0.Models
{
    public class Users
    {
        [Key]
        public int id_user { get; set; }
        public int id_user_type { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public DateTime dob { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public bool is_active { get; set; }



    }
}
