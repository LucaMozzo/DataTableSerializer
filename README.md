[![NuGet](https://img.shields.io/nuget/v/DataTableSerializer.svg)](https://www.nuget.org/packages/DataTableSerializer/)

# DataTableSerializer

A .NET library to serialize an object into a DataTable in a single line via extension method. This saves the time of updating the serialization code whenever the model changes and avoiding doing type mistakes.

## Usage

Import the library
```
using DataTableSerializer;
```

Fill the table with a list of objects
```
Employee employee = new Employee()
{
    FirstName = "fname",
    LastName = "lname",
    DateOfBirth = new DateTime(1970, 1, 1);,
    EmployeeId = 123456
};
var employeeList = new List<Employee> { employee };

DataTable dataTable = new DataTable();
dataTable.Fill<Employee>(employeeList);
```

In the last line, the `Fill` method does the work for you
