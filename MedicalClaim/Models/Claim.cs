namespace MedicalClaim.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public string PatientName { get; set; }
        public DateTime DateOfService { get; set; }
        public string MedicalProvider { get; set; }
        public string Diagnosis { get; set; }
        public decimal ClaimAmount { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
    }
}