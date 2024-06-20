using Microsoft.EntityFrameworkCore;
using PrescriptionApp.Context;
using PrescriptionApp.DTOs;
using PrescriptionApp.Exceptions;
using PrescriptionApp.Models;

namespace PrescriptionApp.Repositories;

public class PharmacyRepository : IPharmacyRepository
{
    private readonly Context.Context _context;

    public PharmacyRepository(Context.Context context)
    {
        _context = context;
    }

    public async Task<Patient> GetPatient(int idPatient)
    {
        return await _context.Patients.FindAsync(idPatient);
    }

    public async Task<int> AddPatient(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        return patient.Id;
    }

    public async Task<IQueryable<Prescription>> GetPatientPrescriptions(int idPatient)
    {
        var result = _context.Prescriptions.Where(e => e.IdPatient == idPatient);
        return result;
    }
    
    public async Task<bool> PatientExist(int idPatient)
    {
        return await _context.Patients.AnyAsync(p => p.Id == idPatient);
    }

    public async Task<bool> MedicineExist(int idMedicine)
    {
        return await _context.Medicaments.AnyAsync(m => m.Id == idMedicine);
    }
    
    public async Task<bool> DoctorExist(int idMedicine)
    {
        return await _context.Medicaments.AnyAsync(m => m.Id == idMedicine);
    }
    

    public async Task<int> CreatePrescription(Prescription prescription)
    {
        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();
        return prescription.Id;
    }

    public async Task<int> AddMedicineToPrescription(int idPrescription, int idMedicament,int dose,string description)
    {
        var prescriptionMedicament = new PrescriptionMedicament()
        {
            IdMedicament = idMedicament,
            IdPrescription = idPrescription,
            Dose = dose,
            Description = description
        };

        await _context.PrescriptionMedicaments.AddAsync(prescriptionMedicament);
        await _context.SaveChangesAsync();
        return idPrescription;
    }

    public async Task<PatientPrescreptionResultDto> getPatientMeds(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Where(p => p.Id == idPatient)
            .FirstOrDefaultAsync();

        if (patient == null)
        {
            throw new PrescriptionException($"Patient with ID {idPatient} not found");
        }

        var prescriptions = patient.Prescriptions
            .OrderBy(p => p.DueDate)
            .Select(p => new PrescriptionsResultDto(
                p.Id,
                p.Date,
                p.DueDate,
                p.PrescriptionMedicaments.Select(pm => new MedicamentDto(
                    pm.Medicament.Id,
                    pm.Medicament.Name,
                    pm.Dose,
                    pm.Description
                )).ToList(),
                new DoctorResultDto(
                    p.Doctor.Id,
                    p.Doctor.FirstName
                )
            )).ToList();

        return new PatientPrescreptionResultDto(
            patient.Id,
            patient.FirstName,
            patient.LastName,
            prescriptions
        );
    }

}