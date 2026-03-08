using DentalClinic.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
      : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseModel>();

            if (_httpContextAccessor.HttpContext != null)
            {
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var entityEntry in entries)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;


                    }
                    else if (entityEntry.State == EntityState.Modified)
                    {
                        entityEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;


                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);

        }


        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseModel>();
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;


                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;


                }
            }

            return base.SaveChanges();
        }

    }
}
