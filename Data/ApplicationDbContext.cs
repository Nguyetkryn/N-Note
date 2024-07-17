using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Note.Models;

namespace Note.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users, Roles, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base (options) 
        {

        }

        //public DbSet<Users> Users { get; set; }
        //public DbSet<Roles> Roles { get; set; }
        public DbSet<Notes> Notes { get; set; }
    }
}