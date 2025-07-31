using Microsoft.AspNetCore.Mvc;
using CodeZone.BLL.Interfaces;
using CodeZone.BLL.DTOs;

namespace CodeZone_Task.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IAttendanceService _attendanceService;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService, IAttendanceService attendanceService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _attendanceService = attendanceService;
        }

        // GET: Employee
        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {
            var paginatedEmployees = await _employeeService.GetEmployeesPaginatedAsync(page, pageSize);
            return View(paginatedEmployees);
        }

        // GET: Employee/GetAllEmployees (AJAX)
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Json(employees);
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            // Get attendance history for this employee
            var attendanceHistory = await _attendanceService.GetAttendanceByEmployeeAsync(id);
            ViewBag.AttendanceHistory = attendanceHistory;

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

            TempData["SuccessMessage"] = _employeeService.GetSuccessMessage("created", "Employee");
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
            employeeDto.Id = id; // Ensure the ID is set for validation
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

            TempData["SuccessMessage"] = _employeeService.GetSuccessMessage("updated", "Employee");
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            TempData["SuccessMessage"] = _employeeService.GetSuccessMessage("deleted", "Employee");
            return RedirectToAction(nameof(Index));
        }
    }
}
