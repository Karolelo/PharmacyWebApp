using PrescriptionApp.DTOs;
using PrescriptionApp.Models;

namespace PrescriptionApp.Repositories;

public interface IPharmacyRepository
{
    Task<PatientPrescreptionResultDto> getPatientMeds(int idPatient);
    Task<Patient> GetPatient(int idPatient);
    Task<int> AddPatient(Patient patient);

    Task<bool> PatientExist(int idPatient);

    Task<bool> MedicineExist(int idMedicine);

    Task<bool> DoctorExist(int idDoctor);

    Task<int> CreatePrescription(Prescription prescription);

    Task<int> AddMedicineToPrescription(int prescriptionId, int idMedicament,int dose,string description);
}