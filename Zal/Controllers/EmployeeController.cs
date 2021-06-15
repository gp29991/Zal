using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zal.Models;
using Zal.ViewModels;

namespace Zal.Controllers
{
    /// <summary>
    /// Controller for dealing with employees (viewing, adding, editing, deleting). Accessible only to logged-in users.
    /// </summary>
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Method for viewing all employees in the database. It gets the list of employees from the "employees" database, sorts it and displays the appropriate view.
        /// </summary>
        /// <param name="sortBy">Property by which employees are sorted.</param>
        /// <param name="sortType">Direction of sorting. "desc" means descending, any other value ("asc" is used as default) means ascending.</param>
        /// <returns>The ViewEmployees view with the sorted employee list as its model.</returns>
        public async Task<IActionResult> ViewEmployees(string sortBy = "FirstName", string sortType = "asc")
        {
            ViewBag.title = "Widok Pracowników";
            ViewMethodViewModel<Employee> model = new ViewMethodViewModel<Employee>("FirstName", "LastName", "Department");
            if(sortType != "desc")
            {
                model.SortTypeForColumns[sortBy] = "desc";
            }
            model.Properties = await _employeeRepository.GetEmployees();
            switch (sortBy)
            {
                case "LastName":
                    model.Properties.Sort((e1, e2) => e1.LastName.CompareTo(e2.LastName));
                    break;
                case "Department":
                    model.Properties.Sort((e1, e2) => e1.Department.Name.CompareTo(e2.Department.Name));
                    break;
                default:
                    model.Properties.Sort((e1, e2) => e1.FirstName.CompareTo(e2.FirstName));
                    break;
            }
            if (sortType == "desc")
            {
                model.Properties.Reverse();
            }
            return View(model);
        }

        /// <summary>
        /// Get method to display the view for adding new employees.
        /// Accessible only to users assigned either to the "Administrator" or the "Manager" role.
        /// </summary>
        /// <returns>The AddEmployee view.</returns>
        [Authorize(Roles = RoleTypes.Administrator + "," + RoleTypes.Manager)]
        [HttpGet]
        public IActionResult AddEmployee()
        {
            ViewBag.title = "Dodawanie Pracownika";
            return View();
        }

        /// <summary>
        /// Post method for actually adding new employees to the database.
        /// Accessible only to users assigned either to the "Administrator" or the "Manager" role.
        /// </summary>
        /// <param name="model">An instance of the Employee class as a validation model for adding new employees.</param>
        /// <returns>Either the AddEmployee view to display validation problems, or a redirect to the ViewEmployees view if the employee was successfully added to the database.</returns>
        [Authorize(Roles = RoleTypes.Administrator + "," + RoleTypes.Manager)]
        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee model)
        {
            ViewBag.title = "Dodawanie Pracownika";
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _employeeRepository.AddEmployee(model);
            return RedirectToAction("ViewEmployees");
        }

        /// <summary>
        /// Get method to display the view for editing new employees.
        /// Accessible only to users assigned either to the "Administrator" or the "Manager" role.
        /// </summary>
        /// <param name="id">The ID of the employee to be edited.</param>
        /// <returns>Either a redirect to the ViewEmployees action if the employee doesn't exist or the EditEmployee view.</returns>
        [Authorize(Roles = RoleTypes.Administrator + "," + RoleTypes.Manager)]
        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            ViewBag.title = "Edycja Pracownika";
            var employee = await _employeeRepository.GetEmployee(id);
            if (employee == null)
            {
                return RedirectToAction("ViewEmployees");
            }
            return View(employee);
        }

        /// <summary>
        /// Post method for updating the edited employee in the database.
        /// Accessible only to users assigned either to the "Administrator" or the "Manager" role.
        /// </summary>
        /// <param name="model">An instance of the Employee class as a validation model for updating employees.</param>
        /// <returns>Either the EditEmployee view to display validation problems, or a redirect to the ViewEmployees action if the employee was successfully updated in the database.</returns>
        [Authorize(Roles = RoleTypes.Administrator + "," + RoleTypes.Manager)]
        [HttpPost]
        public async Task<IActionResult> EditEmployee(Employee model)
        {
            ViewBag.title = "Edycja Pracownika";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _employeeRepository.EditEmployee(model);
            return RedirectToAction("ViewEmployees");
        }

        /// <summary>
        /// Post method for deleting an employee from the database.
        /// Accessible only to users assigned either to the "Administrator" or the "Manager" role.
        /// </summary>
        /// <param name="id">The ID of the employee to be deleted.</param>
        /// <returns>A redirect to the ViewEmployees view.</returns>
        [Authorize(Roles = RoleTypes.Administrator + "," + RoleTypes.Manager)]
        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployee(id);
            if (employee != null)
            {
                await _employeeRepository.DeleteEmployee(employee);
            }
            return RedirectToAction("ViewEmployees");
        }


        /// <summary>
        /// Method for viewing detailed information about a particular employee.
        /// </summary>
        /// <param name="id">The ID of the employee whose details are to be displayed.</param>
        /// <returns>Either a redirect to the ViewEmployees action if the requested employee doesn't exist or the ViewEmployeeDetails view.</returns>
        public async Task<IActionResult> ViewEmployeeDetails(int id)
        {
            ViewBag.title = "Szczegóły Pracownika";
            var employee = await _employeeRepository.GetEmployee(id);
            if (employee == null)
            {
                return RedirectToAction("ViewEmployees");
            }
            return View(employee);
        }
    }
}
