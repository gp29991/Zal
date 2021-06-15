using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zal.Models
{
    /// <summary>
    /// Interface for registering DepartmentRepository as a service for use in dependency injection.
    /// </summary>
    public interface IDepartmentRepository
    {
        Task AddDepartment(Department department);
        Task<Department> GetDepartment(int id);
        Task<List<Department>> GetDepartments();
        Task EditDepartment(Department department);
        Task DeleteDepartment(Department department);
    }
}
