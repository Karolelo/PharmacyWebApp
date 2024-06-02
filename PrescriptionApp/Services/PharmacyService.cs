using PrescriptionApp.DTOs;
using PrescriptionApp.Exceptions;
using PrescriptionApp.Models;
using PrescriptionApp.Repositories;

namespace PrescriptionApp.Service;

public class PharmacyService : IPharmacyService
{
    private readonly IPharmacyRepository _pharmacyRepository;

    public PharmacyService(IPharmacyRepository pharmacyRepository)
    {
        _pharmacyRepository = pharmacyRepository;
    }

    public async Task<int> CreatePrescription(PrescriptionDto prescription)
    {
        if (prescription.Medicaments.Count > 10)
        {
            throw new PrescriptionException("Too many medicaments");
        }

        if (prescription.DueDate <= DateTime.Now)
        {
            throw new PrescriptionException("Prescription date outDated");
        }

        if (!await _pharmacyRepository.DoctorExist(prescription.IdDoctor))
        {
            throw new PrescriptionException($"Doctor with {prescription.IdDoctor} id not exist");
        }
        
        
        int idPatient = prescription.IdPatient;
        
        if (!await _pharmacyRepository.PatientExist(idPatient))
        {
            var patient = new Patient()
            {
                FirstName = prescription.PatientFirstName,
                LastName = prescription.PatientLastName
            };

            idPatient = await _pharmacyRepository.AddPatient(patient);
        }

        foreach (var med in prescription.Medicaments)
        {
            if (!await _pharmacyRepository.MedicineExist(med.IdMedicament))
            {
                throw new PrescriptionException($"Medicament with ID {med.IdMedicament} not exist");
            }
        }

        Prescription pres = new Prescription()
        {
            IdDoctor = prescription.IdDoctor,
            DueDate = prescription.DueDate,
            Date = DateTime.Now,
            IdPatient = prescription.IdPatient
        };


        var idPres = await _pharmacyRepository.CreatePrescription(pres);
        
        foreach (var med in prescription.Medicaments)
        {
            await _pharmacyRepository.AddMedicineToPrescription(idPres, med.IdMedicament, med.Dose, med.Description);
        }
        
        


        return prescription.Medicaments.Count;
    }

    public async Task<PatientPrescreptionResultDto> GetInfoAboutPatient(int idPatient)
    {
        
        var patient = await _pharmacyRepository.GetPatient(idPatient);

        if (patient == null)
        {
            throw new PrescriptionException($"Patient with ID {idPatient} not found");
        }

        var result =await _pharmacyRepository.getPatientMeds(idPatient);

        return result;
    }
}