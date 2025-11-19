using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserAuth.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        //Recibe configuración de connectionstring
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 

        }
        
        //Representa la tabla Users
        public DbSet<User> Users => Set<User>();
        //Representa la tabla Refreshtoken
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        //configuracin de relacion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //relacion user -> refresh token
            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User!)
                .HasForeignKey(rt => rt.UserId);
        }
    }
}
