using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGMH.Healthcare.Vedant.Data.Entities;
using System.Linq;

namespace SGMH.Healthcare.Vedant.Data
{
    public partial class PatientsContext : IdentityDbContext<ApplicationUser>
    {
        public PatientsContext(DbContextOptions<PatientsContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Centre> Centres { get; set; }
        public DbSet<Consultation> Consultation { get; set; }
        public DbSet<Drug> Drug { get; set; }
        public DbSet<DrugCategory> DrugCategory { get; set; }
        public DbSet<PatientImage> PatientImages { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<PrescriptionImage> PrescriptionImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasOne<Address>(a => a.Address)
                .WithMany(p => p.Patients);

            modelBuilder.Entity<Patient>()
                .HasOne<Centre>(c => c.Centre)
                .WithMany(p => p.Patients);
        }
    }
}