using Employees.Models.Response;
using Employees.Requset.Models;

namespace Employees.Services;

public interface IEmployeesService
{
   public Task<List<EmployeesInProject>> GetEmployeesWhichWorkTogether(List<Employee> employees);
}