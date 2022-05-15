using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccess.Base
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) : base(options) { }

        public override int SaveChanges()
        {
            var addedAuditedEntities = ChangeTracker.Entries<EntityBase>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker.Entries<EntityBase>()
                .Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity);

            var now = DateTime.UtcNow;

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedDate = now;
                added.ModifiedDate = now;
                var validationContext = new ValidationContext(added);
                Validator.ValidateObject(added, validationContext);
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                Entry(modified).Property("CreatedDate").IsModified = false;
                modified.ModifiedDate = now;
                var validationContext = new ValidationContext(modified);
                Validator.ValidateObject(modified, validationContext);
            }

            return base.SaveChanges();
        }
    }
}