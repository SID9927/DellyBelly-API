using DellyBelly.Domain.Entities;
using DellyBelly.API.DTOs;

namespace DellyBelly.API.Mappings
{
    public static class EmployeeMapper
    {
        public static EmployeeDto ToDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                Role = employee.Role,
                CreatedAt = employee.CreatedAt
            };
        }
    }
}