using Microsoft.AspNetCore.Mvc;
using CodeZone.BLL.Interfaces;
using CodeZone.BLL.DTOs;
using CodeZone.DAL.Entities;

namespace CodeZone_Task.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public AttendanceController(IAttendanceService attendanceService, IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        // GET: Attendance
        public async Task<IActionResult> Index(int? deptId, int? empId, DateTime? from, DateTime? to, int page = 1, int pageSize = 4)
        {
            var paginatedAttendances = await _attendanceService.GetFilteredAttendancesPaginatedAsync(deptId, empId, from, to, page, pageSize);
            ViewBag.Departments = await _departmentService.GetDepartmentsAsync();
            ViewBag.Employees = await _employeeService.GetAllEmployeesAsync();
            ViewBag.SelectedDeptId = deptId;
            ViewBag.SelectedEmpId = empId;
            ViewBag.FromDate = from;
            ViewBag.ToDate = to;
            ViewBag.PaginationData = paginatedAttendances;
            return View(paginatedAttendances.Items);
        }

        // GET: Attendance/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendance/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Employees = await _employeeService.GetAllEmployeesAsync();
            return View();
        }

        // POST: Attendance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Date,Status")] AttendanceDto attendanceDto)
        {
            var (attendance, validation) = await _attendanceService.AddAsync(attendanceDto);
            
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Employees = await _employeeService.GetAllEmployeesAsync();
                return View(attendanceDto);
            }

            TempData["SuccessMessage"] = _attendanceService.GetSuccessMessage("created", "Attendance record");
            return RedirectToAction(nameof(Details), new { id = attendance!.AttendanceId });
        }

        // GET: Attendance/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var attendanceDto = await _attendanceService.GetDtoByIdAsync(id);
            if (attendanceDto == null)
            {
                return NotFound();
            }

            ViewBag.Employees = await _employeeService.GetAllEmployeesAsync();
            return View(attendanceDto);
        }

        // POST: Attendance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Date,Status")] AttendanceDto attendanceDto)
        {
            var (success, validation) = await _attendanceService.UpdateAsync(id, attendanceDto);
            
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Employees = await _employeeService.GetAllEmployeesAsync();
                return View(attendanceDto);
            }

            TempData["SuccessMessage"] = _attendanceService.GetSuccessMessage("updated", "Attendance record");
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // POST: Attendance/UpdateStatus (AJAX)
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var (success, validation) = await _attendanceService.UpdateStatusAsync(id, status);
            
            if (!validation.IsValid)
            {
                return Json(new { success = false, message = _attendanceService.GetValidationErrorMessage(validation) });
            }

            return Json(new { success = true, message = _attendanceService.GetSuccessMessage("updated", "Status") });
        }

        // GET: Attendance/GetAttendanceStatus (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetAttendanceStatus(int employeeId, DateTime date)
        {
            var attendance = await _attendanceService.GetByEmployeeAndDateAsync(employeeId, date);
            
            if (attendance != null)
            {
                return Json(new { status = attendance.Status.ToString() });
            }
            
            return Json(new { status = "Not marked" });
        }

        // POST: Attendance/QuickUpdate (AJAX)
        [HttpPost]
        public async Task<IActionResult> QuickUpdate(int employeeId, DateTime date, string status)
        {
            var (success, validation) = await _attendanceService.QuickUpdateAsync(employeeId, date, status);
            
            if (!validation.IsValid)
            {
                return Json(new { success = false, message = _attendanceService.GetValidationErrorMessage(validation) });
            }

            return Json(new { success = true, message = $"Attendance marked as {status} successfully!" });
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _attendanceService.DeleteAsync(id);
            TempData["SuccessMessage"] = _attendanceService.GetSuccessMessage("deleted", "Attendance record");
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendance/Search (AJAX)
        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm, int? deptId, int? empId, DateTime? from, DateTime? to)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Return filtered records if search term is empty
                var filteredAttendances = await _attendanceService.FilterAsync(deptId, empId, from, to);
                return PartialView("_AttendanceTableBody", filteredAttendances);
            }

            var searchResults = await _attendanceService.SearchAttendancesAsync(searchTerm, deptId, empId, from, to);
            return PartialView("_AttendanceTableBody", searchResults);
        }
    }
} 