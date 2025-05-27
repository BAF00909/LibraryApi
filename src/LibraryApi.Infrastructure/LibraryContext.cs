using LibraryApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Infrastructure
{
    public class LibraryContext: DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public LibraryContext(DbContextOptions<LibraryContext> options): base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Book>().HasMany(b => b.Authors).WithMany(a => a.Books).UsingEntity(j => j.ToTable("BookAuthor"));
            modelBuilder.Entity<Book>().HasMany(b => b.Loans).WithOne(l => l.Book).HasForeignKey(l => l.BookId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(u => u.Loans).WithOne(l => l.User).HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    Email = "admin@library.com",
                    PasswordHash = "hashedPassword",
                    Role = Role.Admin,
                });
        }
    }
}
