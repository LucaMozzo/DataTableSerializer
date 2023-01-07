using DataTableSerializer;

namespace Tests.Models
{
    public record EmployeeWithAttributesClash
    {
        [DataTableColumnName("EmployeeId")]
        public string? FirstName { get; set; }
        [DataTableColumnName("Last Name")]
        public string? LastName { get; set; }
        [DataTableColumnName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        public int EmployeeId { get; set; }
    }
}
