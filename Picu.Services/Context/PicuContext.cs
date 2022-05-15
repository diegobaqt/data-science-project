using DataAccess.Base;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Picu.Services.Context
{
    public class PicuContext : BaseContext
    {
        public PicuContext(DbContextOptions options) : base(options) { }

        public DbSet<Patient> Patient { get; set; }

        public DbSet<VitalSigns> VitalSigns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var defaultSchema = ConfigurationManager.AppSettings["DefaultSchema"];
            if (!string.IsNullOrEmpty(defaultSchema)) modelBuilder.HasDefaultSchema(defaultSchema);

            modelBuilder.Entity<VitalSigns>().Property(x => x.VitalSignsId).HasDefaultValueSql("NEWID()");

            base.OnModelCreating(modelBuilder);
        }
    }
}
