using DellyBelly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(int id);
    }
}
