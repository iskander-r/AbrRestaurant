using AbrRestaurant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AbrRestaurant.MenuApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Meal> Meals { get; set; }
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> dbContextOptions) 
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MealEntityConfiguration());
            
        }

        public override int SaveChanges()
        {
            ApplyTrackingMetadataOnAddedEntities();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTrackingMetadataOnAddedEntities();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyTrackingMetadataOnAddedEntities();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyTrackingMetadataOnAddedEntities()
        {
            var addedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added && p.Entity is TrackableEntity)
                .Select(p => p.Entity as TrackableEntity)
                .ToList();

            addedEntities.ForEach(ApplyOnAddedMetadata);
        }

        private void ApplyOnAddedMetadata(TrackableEntity trackableEntity)
        {
            var now = DateTime.UtcNow;
            trackableEntity.CreatedOn = now;
        }

        private void ApplyOnUpdatedMetadata(TrackableEntity trackableEntity)
        {
            var now = DateTime.UtcNow;
            trackableEntity.LastModifiedOn = now;
        }
    }

    #region Model configurations
    public class MealEntityConfiguration : IEntityTypeConfiguration<Meal>
    {
        public void Configure(
            EntityTypeBuilder<Meal> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(256);
            builder.Property(p => p.Name).IsRequired();

            builder.Property(p => p.Description).HasMaxLength(1024);
            builder.Property(p => p.Description).IsRequired();

            builder.Property(p => p.Price).IsRequired();

            builder.ApplyTrackableConfiguration();
        }
    }

    internal static class TrackableEntityConfigurationExtension
    {
        public static void ApplyTrackableConfiguration<TEntity>(this EntityTypeBuilder<TEntity> builder) 
            where TEntity : TrackableEntity
        {
            // Has no effect, need to fix. Tried to implement IsRowVersion as concurrency token, but
            // found out that PostgreSQL doesn't support this feature.

            // builder.UseXminAsConcurrencyToken();
        }
    }

    #endregion
}
