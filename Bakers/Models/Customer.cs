namespace Bakers.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? MobileNo { get; set;}
        public DateTime? AddedDate { get; set;}
        public DateTime? EditedDate { get; set; }
    }
}
