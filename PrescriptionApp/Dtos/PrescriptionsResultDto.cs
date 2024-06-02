using System.Runtime.InteropServices.JavaScript;

namespace PrescriptionApp.DTOs;

public record PrescriptionsResultDto(int IdPrescritpion,DateTime Date,DateTime DueDate,List<MedicamentDto> Medicament,DoctorResultDto Doctor);