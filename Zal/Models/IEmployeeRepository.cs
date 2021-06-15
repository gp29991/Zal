using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zal.Models
{
    /// <summary>
    /// Interface for registering EmployeeRepository as a service for use in dependency injection.
    /// </summary>
    public interface IEmployeeRepository
    {
        Task AddEmployee(Employee employee);
        Task<Employee> GetEmployee(int id);
        Task<List<Employee>> GetEmployees();
        Task EditEmployee(Employee employee);
        Task DeleteEmployee(Employee employee);
    }
}
