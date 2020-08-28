using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadSwap.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReadSwap.Data
{
    public class DataAccess : IdentityDbContext<AppUser,IdentityRole,string>
    {
        public DbSet<Profile> Profiles { get; set; }

        public DataAccess(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One to One
            builder.Entity<Profile>().HasIndex(profile => profile.UserId).IsUnique();
        }

    }
}
