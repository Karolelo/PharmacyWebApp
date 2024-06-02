using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrescriptionApp.Models;

namespace PrescriptionApp.Configuration
{
    public class PatientsConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(e => e.IdPatient);
            builder.Property(e => e.IdPatient).ValueGeneratedOnAdd();
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
        }
    }
}