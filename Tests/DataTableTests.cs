using System.Data;
using Tests.Models;
using DataTableSerializer;
using DataTableSerializer.Exceptions;
using DbDataReaderMapper;

namespace Tests
{
    [TestClass]
    public class DataTableTests
    {
        [TestMethod]
        public void LoadDataTableSingleElementTest()
        {
            var dob = new DateTime(1970, 1, 1);
            Employee employee = new Employee()
            {
                FirstName = "fname",
                LastName = "lname",
                DateOfBirth = dob,
                EmployeeId = 123456
            };
            var employeeList = new List<Employee> { employee };

            DataTable dataTable = new DataTable();
            dataTable.Fill<Employee>(employeeList);

            Assert.AreEqual(4, dataTable.Columns.Count);
            Assert.AreEqual(1, dataTable.Rows.Count);
            var dataRows = dataTable.AsEnumerable();
            var firstRow = dataRows.First();
            Assert.AreEqual("fname", firstRow["FirstName"] as string);
            Assert.AreEqual("lname", firstRow["LastName"] as string);
            Assert.AreEqual(dob, Convert.ToDateTime(firstRow["DateOfBirth"]));
            Assert.AreEqual(123456, Convert.ToInt32(firstRow["EmployeeId"]));
        }

        [TestMethod]
        public void LoadDataTableSingleElementWithAttributesTest()
        {
            var dob = new DateTime(1970, 1, 1);
            EmployeeWithAttributes employee = new EmployeeWithAttributes()
            {
                FirstName = "fname",
                LastName = "lname",
                DateOfBirth = dob,
                EmployeeId = 123456
            };
            var employeeList = new List<EmployeeWithAttributes> { employee };

            DataTable dataTable = new DataTable();
            dataTable.Fill<EmployeeWithAttributes>(employeeList);

            Assert.AreEqual(4, dataTable.Columns.Count);
            Assert.AreEqual(1, dataTable.Rows.Count);
            var dataRows = dataTable.AsEnumerable();
            var firstRow = dataRows.First();
            Assert.AreEqual("fname", firstRow["First Name"] as string);
            Assert.AreEqual("lname", firstRow["Last Name"] as string);
            Assert.AreEqual(dob, Convert.ToDateTime(firstRow["Date of Birth"]));
            Assert.AreEqual(123456, Convert.ToInt32(firstRow["Employee Id"]));
        }

        [TestMethod]
        public void LoadDataTableShouldFailIfColumnsClash()
        {
            var dob = new DateTime(1970, 1, 1);
            EmployeeWithAttributesClash employee = new EmployeeWithAttributesClash()
            {
                FirstName = "fname",
                LastName = "lname",
                DateOfBirth = dob,
                EmployeeId = 123456
            };
            var employeeList = new List<EmployeeWithAttributesClash> { employee };

            DataTable dataTable = new DataTable();
            Assert.ThrowsException<DataTableColumnNameClashException>(() =>
                dataTable.Fill<EmployeeWithAttributesClash>(employeeList));
        }

        [TestMethod]
        public void LoadDataTableCustomPropertyConverter()
        {
            var dob = new DateTime(1970, 1, 1);
            EmployeeNullableDob employee = new EmployeeNullableDob()
            {
                FirstName = "fname",
                LastName = "lname",
                DateOfBirth = dob,
                EmployeeId = 123456
            };
            var employeeList = new List<EmployeeNullableDob> { employee };

            var converter = new PropertyTransformer()
                .AddTransformation<EmployeeNullableDob, DateTime?, string?>(e => e.DateOfBirth, dob => dob?.ToLongDateString());

            DataTable dataTable = new DataTable();
            dataTable.Fill<EmployeeNullableDob>(employeeList, converter);

            Assert.AreEqual(4, dataTable.Columns.Count);
            Assert.AreEqual(1, dataTable.Rows.Count);
            var dataRows = dataTable.AsEnumerable();
            var firstRow = dataRows.First();
            Assert.AreEqual("fname", firstRow["FirstName"] as string);
            Assert.AreEqual("lname", firstRow["LastName"] as string);
            Assert.AreEqual(dob.ToLongDateString(), firstRow["DateOfBirth"] as string);
            Assert.AreEqual(123456, Convert.ToInt32(firstRow["EmployeeId"]));
        }
    }
}