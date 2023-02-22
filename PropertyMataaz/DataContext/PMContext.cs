using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.Models;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.UtilityModels;

namespace PropertyMataaz.DataContext
{
    public class PMContext : IdentityDbContext<User, Role, int>
    {
        public PMContext(DbContextOptions<PMContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add your customizations after calling base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<User>()
                .Property(u => u.DateCreated)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<User>()
                .Property(u => u.DateModified)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Property>().HasOne(p => p.CreatedByUser).WithMany(u => u.Properties);

            modelBuilder.Entity<Application>().HasOne(a => a.User).WithMany(u => u.Applications).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PropertyRequestMatch>().HasOne(p => p.PropertyRequest).WithMany(p => p.Matches).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasOne(t => t.User).WithMany(u => u.Transactions).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Inspections>().HasOne(i => i.Property).WithMany(t => t.Inspections).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Inspections>().HasOne(i => i.User).WithMany(u => u.Inspections).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Media>().HasOne(p => p.Property).WithMany(p => p.MediaFiles).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RentRelief>().HasOne(p => p.User).WithMany(p => p.RentReliefs).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Report>().HasOne(p => p.User).WithMany(p => p.Reports).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Transaction>().HasOne(t => t.Tenancy).WithOne(t => t.Transaction).HasForeignKey<Tenancy>(t => t.TransactionId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Tenancy>().HasOne(t => t.Owner).WithMany(u => u.Tenancies).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Tenancy>().HasOne(t => t.Tenant).WithMany(u => u.MyTenancies).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CleaningQuote>().HasOne(c => c.Cleaning).WithMany(c => c.CleaningQuotes).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Complaints>().HasOne(c => c.User).WithMany(u => u.Complaints).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<InspectionTime>().HasOne(i => i.InspectionDate).WithMany(i => i.Times).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<InspectionDate>().HasMany(i => i.Times).WithOne(i => i.InspectionDate).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        }

        public DbSet<Code> Codes { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<PropertyRequest> PropertyRequests { get; set; }
        public DbSet<UserEnquiry> UserEnquiries { get; set; }
        public DbSet<InspectionDate> InspectionDates { get; set; }
        public DbSet<InspectionTime> InspectionTimes { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<NextOfKin> NextOfKins { get; set; }
        public DbSet<PropertyRequestMatch> PropertyRequestMatches { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentLog> PaymentLogs { get; set; }
        public DbSet<TenantType> TenantTypes { get; set; }
        public DbSet<RentCollectionType> RentCollectionTypes { get; set; }
        public DbSet<Inspections> Inspections { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Cleaning> Cleanings { get; set; }
        public DbSet<CleaningQuote> CleaningQuotes { get; set; }
        public DbSet<LandSearch> LandSearches { get; set; }
        public DbSet<Tenancy> Tenancies { get; set; }
        public DbSet<RentRelief> RentReliefs { get; set; }
        public DbSet<Complaints> Complaints { get; set; }
        public DbSet<ComplaintsCategory> ComplaintsCategories { get; set; }
        public DbSet<ComplaintsSubCategory> ComplaintsSubCategories { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<PropertyTitle> PropertyTitles { get; set; }

    }
}
