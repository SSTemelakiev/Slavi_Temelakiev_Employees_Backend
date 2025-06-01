using Employees.Requset.Models;
using Employees.Services;
using FluentAssertions;
using Xunit;

namespace Employees.Tests;

public class EmployeesServiceTests
{
    private readonly EmployeesService _employeesService = new();

    [Fact]
    public async Task GetEmployeesWhichWorkTogether_ShouldReturnCorrectPair_WhenOverlapExists()
    {
        var employees = new List<Employee>
        {
            new Employee
            {
                EmpID = 1,
                ProjectID = 100,
                DateFrom = new DateTime(2020, 01, 01),
                DateTo = new DateTime(2020, 06, 01)
            },
            new Employee
            {
                EmpID = 2,
                ProjectID = 100,
                DateFrom = new DateTime(2020, 03, 01),
                DateTo = new DateTime(2020, 08, 01)
            }
        };
        
        var result = await _employeesService.GetEmployeesWhichWorkTogether(employees);
        
        result.Should().HaveCount(1);
        result[0].FirstEmployeeID.Should().Be(1);
        result[0].SecondEmployeeID.Should().Be(2);
        result[0].ProjectID.Should().Be(100);
        result[0].DaysWorked.Should().Be(93); 
    }
    
    [Fact]
    public async Task GetEmployeesWhichWorkTogether_ShouldReturnEmpty_WhenNoOverlap()
    {
        var employees = new List<Employee>
        {
            new Employee
            {
                EmpID = 1,
                ProjectID = 101,
                DateFrom = new DateTime(2020, 01, 01),
                DateTo = new DateTime(2020, 02, 01)
            },
            new Employee
            {
                EmpID = 2,
                ProjectID = 101,
                DateFrom = new DateTime(2020, 03, 01),
                DateTo = new DateTime(2020, 04, 01)
            }
        };

        var result = await _employeesService.GetEmployeesWhichWorkTogether(employees);

        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetEmployeesWhichWorkTogether_ShouldHandleNullDateTo_AsToday()
    {
        var employees = new List<Employee>
        {
            new Employee
            {
                EmpID = 1,
                ProjectID = 200,
                DateFrom = DateTime.Today.AddDays(-10),
                DateTo = null
            },
            new Employee
            {
                EmpID = 2,
                ProjectID = 200,
                DateFrom = DateTime.Today.AddDays(-5),
                DateTo = null
            }
        };

        var result = await _employeesService.GetEmployeesWhichWorkTogether(employees);

        result.Should().HaveCount(1);
        result[0].DaysWorked.Should().Be(6);
    }
}