namespace PrescriptionApp.Models;

public class Doctor : BasicEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; }
}