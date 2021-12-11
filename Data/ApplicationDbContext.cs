using System.IO;
using Common.Utilities;
using Entities;
using Entities.Common;
using Entities.Role;
using Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<
       User,
       Role,
       long,
       IdentityUserClaim<long>,
       UserRole,
       IdentityUserLogin<long>,
       IdentityRoleClaim<long>,
       IdentityUserToken<long>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //        .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AccountManager"))
        //        .AddJsonFile("appsettings.json")
        //        .AddJsonFile($"appsettings.Development.json", optional: true)
        //        .Build();

        //    var connectionString = configuration["ConnectionStrings:Development"];
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer(connectionString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entitiesConfig = typeof(IEntity).Assembly;
            var entitiesAssembly = typeof(IEntity).Assembly;

            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesConfig);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();
            modelBuilder.AddPluralizingTableNameConvention();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasOne(e => e.Role)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.RoleId)
                .IsRequired();

            modelBuilder.Entity<UserRole>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}