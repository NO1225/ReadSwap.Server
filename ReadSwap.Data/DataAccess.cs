using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadSwap.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReadSwap.Data
{
    public class DataAccess : IdentityDbContext<AppUser,IdentityRole,string>
    {
        public DataAccess(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Profile> Profiles { get; set; }
    }
}
