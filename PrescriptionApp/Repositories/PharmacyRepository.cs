using Microsoft.EntityFrameworkCore;
using PrescriptionApp.Context;
using PrescriptionApp.DTOs;
using PrescriptionApp.Exceptions;
using PrescriptionApp.Models;

namespace PrescriptionApp.Repositories;

public class PharmacyRepository : IPharmacyRepository
{
    private readonly PharmacyContext _pharmacyContext;

    public PharmacyRepository(PharmacyContext pharmacyContext)
    {
        _pharmacyContext = pharmacyContext;
    }

    public async Task<Patient> GetPatient(int idPatient)
    {
        return await _pharmacyContext.Patients.FindAsync(idPatient);
    }

    public async Task<int> AddPatient(Patient patient)
    {
        await _pharmacyContext.Patients.AddAsync(patient);
        await _pharmacyContext.SaveChangesAsync();

        return patient.IdPatient;
    }

    public async Task<IQueryable<Prescription>> GetPatientPrescriptions(int idPatient)
    {
        var result = _pharmacyContext.Prescriptions.Where(e => e.IdPatient == idPatient);
        return result;
    }
    
    public async Task<bool> PatientExist(int idPatient)
    {
        return await _pharmacyContext.Patients.AnyAsync(p => p.IdPatient == idPatient);
    }

    public async Task<bool> MedicineExist(int idMedicine)
    {
        return await _pharmacyContext.Medicaments.AnyAsync(m => m.IdMedicament == idMedicine);
    }
    
    public async Task<bool> DoctorExist(int idMedicine)
    {
        return await _pharmacyContext.Medicaments.AnyAsync(m => m.IdMedicament == idMedicine);
    }
    

    public async Task<int> CreatePrescription(Prescription prescription)
    {
        await _pharmacyContext.Prescriptions.AddAsync(prescription);
        await _pharmacyContext.SaveChangesAsync();
        return prescription.IdPrescription;
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

        await _pharmacyContext.PrescriptionMedicaments.AddAsync(prescriptionMedicament);
        await _pharmacyContext.SaveChangesAsync();
        return idPrescription;
    }

    public async Task<PatientPrescreptionResultDto> getPatientMeds(int idPatient)
    {
        var patient = await _pharmacyContext.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Where(p => p.IdPatient == idPatient)
            .FirstOrDefaultAsync();

        if (patient == null)
        {
            throw new PrescriptionException($"Patient with ID {idPatient} not found");
        }

        var prescriptions = patient.Prescriptions
            .OrderBy(p => p.DueDate)
            .Select(p => new PrescriptionsResultDto(
                p.IdPrescription,
                p.Date,
                p.DueDate,
                p.PrescriptionMedicaments.Select(pm => new MedicamentDto(
                    pm.Medicament.IdMedicament,
                    pm.Medicament.Name,
                    pm.Dose,
                    pm.Description
                )).ToList(),
                new DoctorResultDto(
                    p.Doctor.IdDoctor,
                    p.Doctor.FirstName
                )
            )).ToList();

        return new PatientPrescreptionResultDto(
            patient.IdPatient,
            patient.FirstName,
            patient.LastName,
            prescriptions
        );
    }

}