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

        public DbSet<Book> Books { get; set; }

        public DbSet<BookImage> BookImages { get; set; }

        public DataAccess(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One to One
            builder.Entity<Profile>().HasIndex(profile => profile.UserId).IsUnique();

            builder.Entity<Book>().HasIndex(book => book.BookImageId).IsUnique();
            builder.Entity<Book>().HasOne<BookImage>(b => b.BookImage).WithOne().HasForeignKey<Book>(b=>b.BookImageId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<BookImage>().HasOne<Book>(bi => bi.Book).WithMany(b => b.BookImages).OnDelete(DeleteBehavior.NoAction);
        }

    }
}
