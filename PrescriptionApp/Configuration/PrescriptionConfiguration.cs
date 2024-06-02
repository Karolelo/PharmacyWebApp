using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrescriptionApp.Models;

namespace PrescriptionApp.Configuration
{
    public class PrescriptionsConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(e => e.IdPrescription);
            builder.Property(e => e.IdPrescription).ValueGeneratedOnAdd();
            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.DueDate).IsRequired();

            builder.HasOne(e => e.Patient)
                .WithMany(p => p.Prescriptions)  
                .HasForeignKey(e => e.IdPatient);

            builder.HasOne(e => e.Doctor)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(e => e.IdDoctor);
        }
    }
}