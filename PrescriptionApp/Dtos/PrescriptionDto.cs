using System.Runtime.InteropServices.JavaScript;
using Microsoft.VisualBasic;

namespace PrescriptionApp.DTOs;

public record PrescriptionDto(int IdPatient,string PatientFirstName,string PatientLastName,int IdDoctor,DateTime DueDate,List<MedicamentDto>Medicaments);