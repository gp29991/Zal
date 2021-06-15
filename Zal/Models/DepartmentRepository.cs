using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zal.Data;

namespace Zal.Models
{
    /// <summary>
    /// Repository providing basic CRUD functionality for the "Departments" table in the "employee" database.
    /// Implements IDepartmentRepository for the purposes of dependency injection.
    /// </summary>
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly EmployeeContext _context;
        public DepartmentRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task AddDepartment(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task<Department> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            return department;
        }

        public async Task<List<Department>> GetDepartments()
        {
            var departments = await _context.Departments.ToListAsync();
            return departments;
        }

        public async Task EditDepartment(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartment(Department department)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
