using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrescriptionApp.Models;

namespace PrescriptionApp.Configuration
{
    public class PrescriptionMedicamentsConfiguration : IEntityTypeConfiguration<PrescriptionMedicament>
    {
        public void Configure(EntityTypeBuilder<PrescriptionMedicament> builder)
        {
            builder.HasKey(e => new { e.IdPrescription, e.IdMedicament });

            builder.Property(e => e.Dose).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(200);

            builder.HasOne(e => e.Prescription)
                .WithMany(p => p.PrescriptionMedicaments)
                .HasForeignKey(e => e.IdPrescription);

            builder.HasOne(e => e.Medicament)
                .WithMany(m => m.PrescriptionMedicaments)
                .HasForeignKey(e => e.IdMedicament);
        }
    }
}