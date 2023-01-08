[![NuGet](https://img.shields.io/nuget/v/DataTableSerializer.svg)](https://www.nuget.org/packages/DataTableSerializer/)

# DataTableSerializer

A .NET library to serialize an object into a DataTable in a single line via extension method. This saves the time of updating the serialization code whenever the model changes and avoiding doing type mistakes.

## Basic usage

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

## Property transformations

`DataTable` doesn't support nullable types, so sometimes it could be handy to add a transformation to the properties.

The example below shows how to use a `PropertyTransfomer` to map a `DateTime?` (not allowed because it's nullable) to a `string`.
```
var converter = new PropertyTransformer()
    .AddTransformation<EmployeeNullableDob, DateTime?, string?>(e => e.DateOfBirth, dob => dob?.ToLongDateString());

DataTable dataTable = new DataTable();
dataTable.Fill<EmployeeNullableDob>(employeeList, converter);
```

Look at the tests to see this example in action.
