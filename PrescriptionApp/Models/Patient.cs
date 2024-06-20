using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescriptionApp.Models;

public class Patient : BasicEntity
{   
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDay { get; set; }
    
    public ICollection<Prescription> Prescriptions { get; set; }

    public Patient()
    {
    }

    public Patient(int idPatient, string firstName, string lastName, DateOnly birthDay)
    {
        Id = idPatient;
        FirstName = firstName;
        LastName = lastName;
        BirthDay = birthDay;
    }
}