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
    /// Controller for dealing with departments (viewing, adding, editing, deleting). Accessible only to users assigned either to the "Administrator" or the "Manager" role.
    /// </summary>
    [Authorize(Roles = RoleTypes.Administrator + "," + RoleTypes.Manager)]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        /// <summary>
        /// Method for viewing all departments in the database. It gets the list of departments from the "employees" database, sorts it and displays the appropriate view.
        /// </summary>
        /// <param name="sortType">Direction of sorting. "desc" means descending, any other value ("asc" is used as default) means ascending.</param>
        /// <returns>The ViewDepartments view with the sorted department list as its model.</returns>
        public async Task<IActionResult> ViewDepartments(string sortType="asc")
        {
            ViewBag.title = "Widok Wydziałów";
            ViewMethodViewModel<Department> model = new ViewMethodViewModel<Department>("Name");
            if (sortType != "desc")
            {
                model.SortTypeForColumns["Name"] = "desc";
            }
            model.Properties = await _departmentRepository.GetDepartments();
            model.Properties.Sort((d1, d2) => d1.Name.CompareTo(d2.Name));
            if (sortType == "desc")
            {
                model.Properties.Reverse();
            }
            return View(model);
        }

        /// <summary>
        /// Get method to display the view for adding new departments.
        /// </summary>
        /// <returns>The AddDepartment view.</returns>
        [HttpGet]
        public IActionResult AddDepartment()
        {
            ViewBag.title = "Dodawanie Wydziału";
            return View();
        }

        /// <summary>
        /// Post method for actually adding new departments to the database.
        /// </summary>
        /// <param name="model">An instance of the Department class as a validation model for adding new departments.</param>
        /// <returns>Either the AddDepartment view to display validation problems, or a redirect to the ViewDepartments view if the department was successfully added to the database.</returns>
        [HttpPost]
        public async Task<IActionResult> AddDepartment(Department model)
        {
            ViewBag.title = "Dodawanie Wydziału";

            if (!ModelState.IsValid)
            {
                return View();
            }
            var departments = await _departmentRepository.GetDepartments();
            if (departments.Any(department => department.Name == model.Name))
            {
                ModelState.AddModelError("DepartmentAlreadyExists", "Wydział o podanej nazwie już istnieje");
                return View();
            }
            await _departmentRepository.AddDepartment(model);
            return RedirectToAction("ViewDepartments");
        }

        /// <summary>
        /// Get method to display the view for editing new departments.
        /// </summary>
        /// <param name="id">The ID of the department to be edited.</param>
        /// <returns>Either a redirect to the ViewDepartments action if the department doesn't exist or the EditDepartment view.</returns>
        [HttpGet]
        public async Task<IActionResult> EditDepartment(int id)
        {
            ViewBag.title = "Edycja Wydziału";
            var department = await _departmentRepository.GetDepartment(id);
            if (department == null)
            {
                return RedirectToAction("ViewDepartments");
            }
            return View(department);
        }

        /// <summary>
        /// Post method for updating the edited department in the database.
        /// </summary>
        /// <param name="model">An instance of the Department class as a validation model for updating departments.</param>
        /// <returns>Either the EditDepartment view to display validation problems, or a redirect to the ViewDepartments action if the department was successfully updated in the database.</returns>
        [HttpPost]
        public async Task<IActionResult> EditDepartment(Department model)
        {
            ViewBag.title = "Edycja Wydziału";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var departments = await _departmentRepository.GetDepartments();
            if (departments.Any(department => department.Name == model.Name && department.ID != model.ID))
            {
                ModelState.AddModelError("DepartmentAlreadyExists", "Wydział o podanej nazwie już istnieje");
                return View(model);
            }
            await _departmentRepository.EditDepartment(model);
            return RedirectToAction("ViewDepartments");
        }

        /// <summary>
        /// Post method for deleting a department from the database.
        /// </summary>
        /// <param name="id">The ID of the department to be deleted.</param>
        /// <returns>A redirect to the ViewDepartments view.</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _departmentRepository.GetDepartment(id);
            if(department != null)
            {
                await _departmentRepository.DeleteDepartment(department);
            }
            return RedirectToAction("ViewDepartments");
        }
    }
}
