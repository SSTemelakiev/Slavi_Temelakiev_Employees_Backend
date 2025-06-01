using Employees.Models.Response;
using Employees.Requset.Models;

namespace Employees.Services;

public class EmployeesService : IEmployeesService
{
    public async Task<List<EmployeesInProject>> GetEmployeesWhichWorkTogether(List<Employee> employees)
    {
        return await Task.Run(() =>
        {
            var allEmployeePairsWorkTogetherInProject = employees
                .GroupBy(emp => emp.ProjectID)
                .SelectMany(employeesInProjectGroup =>
                    from firstEmployee in employeesInProjectGroup
                    from secondEmployee in employeesInProjectGroup
                    where firstEmployee.EmpID < secondEmployee.EmpID
                    let overlapDays = GetOverlapDays(firstEmployee.DateFrom, firstEmployee.DateTo,
                        secondEmployee.DateFrom, secondEmployee.DateTo)
                    where overlapDays > 0
                    select new EmployeesInProject
                    {
                        FirstEmployeeID = firstEmployee.EmpID,
                        SecondEmployeeID = secondEmployee.EmpID,
                        ProjectID = employeesInProjectGroup.Key,
                        DaysWorked = overlapDays
                    })
                .OrderByDescending(employeesInProjectGroup => employeesInProjectGroup.DaysWorked)
                .ToList();

            return allEmployeePairsWorkTogetherInProject;
        });
    }

    private static int GetOverlapDays(DateTime dateFrom1, DateTime? dateTo1, DateTime dateFrom2, DateTime? dateTo2)
    {
        dateTo1 = dateTo1 ?? DateTime.Now;
        dateTo2 = dateTo2 ?? DateTime.Now;

        var overlapDateFrom = dateFrom1 > dateFrom2 ? dateFrom1 : dateFrom2;
        var overlapDateTo = dateTo1 < dateTo2 ? dateTo1 : dateTo2;
        return (overlapDateTo.GetValueOrDefault() - overlapDateFrom).Days >= 0
            ? (overlapDateTo.GetValueOrDefault() - overlapDateFrom).Days + 1
            : 0;
    }
}