using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zal.Data;

namespace Zal.Models
{
    /// <summary>
    /// Repository providing basic CRUD functionality for the "Employees" table in the "employee" database.
    /// Implements IEmployeeRepository for the purposes of dependency injection.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;
        public EmployeeRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include("Address")
                .Include("Department")
                .FirstOrDefaultAsync(employee => employee.ID == id);
            return employee;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            var employees = await _context.Employees
                .Include("Address")
                .Include("Department")
                .ToListAsync();
            return employees;
        }

        public async Task EditEmployee(Employee employee)
        {
            var address = employee.Address;
            _context.Employees.Update(employee);
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployee(Employee employee)
        {
            var address = employee.Address;
            _context.Employees.Remove(employee);
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }
}
