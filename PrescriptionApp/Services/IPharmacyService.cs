using PrescriptionApp.DTOs;

namespace PrescriptionApp.Service;
//Future tasks will be implemented
public interface IPharmacyService
{
    Task<int> CreatePrescription(PrescriptionDto prescription);

    Task<PatientPrescreptionResultDto> GetInfoAboutPatient(int idPatient);
}