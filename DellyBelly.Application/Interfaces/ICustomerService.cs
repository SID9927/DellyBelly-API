using DellyBelly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);

    }
}
