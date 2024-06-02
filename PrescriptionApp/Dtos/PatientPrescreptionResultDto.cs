namespace PrescriptionApp.DTOs;

public record PatientPrescreptionResultDto(int IdPatient,string FirstName,string LastName,List<PrescriptionsResultDto> Prescriptons);