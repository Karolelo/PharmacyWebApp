using Microsoft.AspNetCore.Mvc;
using PrescriptionApp.Context;
using PrescriptionApp.DTOs;
using PrescriptionApp.Models;
using PrescriptionApp.Service;

namespace PrescriptionApp.Controllers;
[ApiController]
[Route("api")]
public class PharmacyController : ControllerBase
{
    private readonly IPharmacyService _service;

    public PharmacyController(IPharmacyService service)
    {
        _service = service;
    }
    
    [HttpPost("prescription/create")]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionDto prescriptionDto)
    {
        await _service.CreatePrescription(prescriptionDto);

        return Ok($"Prescription created, {prescriptionDto.Medicaments.Count} med prescribe for patient {prescriptionDto.PatientFirstName} {prescriptionDto.PatientLastName}");
    }
    
    [HttpGet("{idPatient}/meds")]
    public async Task<IActionResult> GetPatientMeds(int idPatient)
    {
        var result = await _service.GetInfoAboutPatient(idPatient);
        return Ok(result);
        
    }
    
}