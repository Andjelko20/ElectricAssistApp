using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {

        private readonly FullStckDbContext _fullStckDbContext;
        public EmployeesController(FullStckDbContext fullStackDbContext)
        {
            _fullStckDbContext = fullStackDbContext;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _fullStckDbContext.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employees employeeRequest)
        {
            employeeRequest.Id = Guid.NewGuid();

            await _fullStckDbContext.Employees.AddAsync(employeeRequest);
            await _fullStckDbContext.SaveChangesAsync();

            return Ok(employeeRequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employee = await _fullStckDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, Employees updateEmployeeRequest)
        {
            var employee = await _fullStckDbContext.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();
            employee.Name = updateEmployeeRequest.Name;
            employee.Email = updateEmployeeRequest.Email;
            employee.Phone = updateEmployeeRequest.Phone;
            employee.Country = updateEmployeeRequest.Country;
            await _fullStckDbContext.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await _fullStckDbContext.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            _fullStckDbContext.Employees.Remove(employee);
            await _fullStckDbContext.SaveChangesAsync();

            return Ok(employee);
        }
    }
}
