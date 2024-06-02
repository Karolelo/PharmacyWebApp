﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrescriptionApp.Models;

namespace PrescriptionApp.Configuration;

public class DoctorsConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(e => e.IdDoctor);
        builder.Property(e => e.IdDoctor).ValueGeneratedOnAdd();
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(20);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(30);
    }
}