using Employees.Models.Response;
using Employees.Requset.Models;
using Employees.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class EmployeesController(IEmployeesService employeesService) : ControllerBase
{
   [HttpPost]
   public async Task<ActionResult<List<EmployeesInProject>>> GetEmployeesWhichWorkTogether(List<Employee> employees)
    => Ok(await employeesService.GetEmployeesWhichWorkTogether(employees));
}