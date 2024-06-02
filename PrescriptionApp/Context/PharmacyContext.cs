﻿using Microsoft.EntityFrameworkCore;
using PrescriptionApp.Configuration;
using PrescriptionApp.Models;

namespace PrescriptionApp.Context;

public class PharmacyContext :DbContext
{
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    protected PharmacyContext()
    {
    }

    public PharmacyContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(PharmacyContext).Assembly);
        modelBuilder.ApplyConfiguration(new PatientsConfiguration());
        modelBuilder.ApplyConfiguration(new DoctorsConfiguration());
        modelBuilder.ApplyConfiguration(new MedicamentsConfiguration());
        modelBuilder.ApplyConfiguration(new PrescriptionsConfiguration());
        modelBuilder.ApplyConfiguration(new PrescriptionMedicamentsConfiguration());
    }
}