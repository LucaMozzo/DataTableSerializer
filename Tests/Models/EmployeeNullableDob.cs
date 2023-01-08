namespace Tests.Models
{
    public record EmployeeNullableDob
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int EmployeeId { get; set; }
    }
}
