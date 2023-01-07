using DataTableSerializer;

namespace Tests.Models
{
    public record EmployeeWithAttributes
    {
        [DataTableColumnName("First Name")]
        public string? FirstName { get; set; }
        [DataTableColumnName("Last Name")]
        public string? LastName { get; set; }
        [DataTableColumnName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [DataTableColumnName("Employee Id")]
        public int EmployeeId { get; set; }
    }
}
