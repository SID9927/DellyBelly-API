using DellyBelly.API.Mappings;
using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DellyBelly.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "super_admin,admin")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllAsync();
            var dtos = employees.Select(EmployeeMapper.ToDto);  // Map each employee to DTO
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();
            return Ok(EmployeeMapper.ToDto(employee));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            var created = await _employeeService.CreateAsync(employee);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest();
            var updated = await _employeeService.UpdateAsync(employee);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
