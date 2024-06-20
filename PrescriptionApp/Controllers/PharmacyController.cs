using GakkoHorizontalSlice.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PrescriptionApp.Context;
using PrescriptionApp.DTOs;
using PrescriptionApp.Models;
using PrescriptionApp.Service;
using RegisterRequest = PrescriptionApp.Models.RegisterRequest;

namespace PrescriptionApp.Controllers;
[ApiController]
[Route("api")]
public class PharmacyController : ControllerBase
{
    private readonly IPharmacyService _pharmacy;
    private readonly IUserService _user;

    public PharmacyController(IPharmacyService pharmacy, IUserService user)
    {
        _pharmacy = pharmacy;
        _user = user;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterStudent(RegisterRequest model)
    {
        await _user.RegisterUser(model);

        return Ok("User created succesfully");
    }
    [Authorize]
    [HttpPost("prescription/create")]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionDto prescriptionDto)
    {
        await _pharmacy.CreatePrescription(prescriptionDto);

        return Ok($"Prescription created, {prescriptionDto.Medicaments.Count} med prescribe for patient {prescriptionDto.PatientFirstName} {prescriptionDto.PatientLastName}");
    }
    [Authorize]
    [HttpGet("{idPatient}/meds")]
    public async Task<IActionResult> GetPatientMeds(int idPatient)
    {
        var result = await _pharmacy.GetInfoAboutPatient(idPatient);
        return Ok(result);
        
    }
    
}