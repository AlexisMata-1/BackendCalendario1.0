using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackendCalendario1._0.Models;

    public class CalendarioContext : DbContext
    {
        public CalendarioContext (DbContextOptions<CalendarioContext> options)
            : base(options)
        {
        }

        public DbSet<BackendCalendario1._0.Models.Users> Users { get; set; }

        public DbSet<BackendCalendario1._0.Models.RegisterDay> RegisterDay { get; set; }

        public DbSet<BackendCalendario1._0.Models.Login> Login { get; set; }

        public DbSet<BackendCalendario1._0.Models.Reports> Reports { get; set; }
    }
