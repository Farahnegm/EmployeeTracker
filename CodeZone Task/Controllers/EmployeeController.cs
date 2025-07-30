using Microsoft.AspNetCore.Mvc;
using CodeZone.BLL.Interfaces;
using CodeZone.BLL.DTOs;

namespace CodeZone_Task.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return View(employees);
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _departmentService.GetDepartmentsAsync();
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Email,DepartmentId")] EmployeeDto employeeDto)
        {
            var (employee, validation) = await _employeeService.AddEmployeeAsync(employeeDto);
            
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Departments = await _departmentService.GetDepartmentsAsync();
                return View(employeeDto);
            }

            TempData["SuccessMessage"] = "Employee created successfully!";
            return RedirectToAction(nameof(Details), new { id = employee!.Id});
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Departments = await _departmentService.GetDepartmentsAsync();
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FullName,Email,DepartmentId")] EmployeeDto employeeDto)
        {
            var (success, validation) = await _employeeService.UpdateEmployeeAsync(id, employeeDto);
            
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Departments = await _departmentService.GetDepartmentsAsync();
                return View(employeeDto);
            }

            TempData["SuccessMessage"] = "Employee updated successfully!";
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            TempData["SuccessMessage"] = "Employee deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
