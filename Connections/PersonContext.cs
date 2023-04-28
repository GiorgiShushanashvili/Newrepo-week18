using System;
using Microsoft.EntityFrameworkCore;
using Mixed.Models;

namespace Mixed.Connections
{
	public class PersonContext:DbContext
	{
        public PersonContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
  => options.UseSqlServer("Server=.;Database=Freshdatabase;User=sa;Password=Giorgi1999;TrustServerCertificate=True");

        public PersonContext(DbContextOptions<PersonContext> option)
              : base(option)
        {

        }
       public DbSet<Person> Persons { get; set; }
       public DbSet<Address> Addresses { get; set; }
       public DbSet<Role> Role { get; set; }
       public DbSet<LoginUser> LoginUser { get; set; }

    }
}

