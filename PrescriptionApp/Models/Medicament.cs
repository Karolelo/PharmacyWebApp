namespace PrescriptionApp.Models;

public class Medicament : BasicEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}