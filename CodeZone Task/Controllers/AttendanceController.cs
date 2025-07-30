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
        public async Task<IActionResult> Index(int? deptId, int? empId, DateTime? from, DateTime? to)
        {
            var attendances = await _attendanceService.FilterAsync(deptId, empId, from, to);
            ViewBag.Departments = await _departmentService.GetDepartmentsAsync();
            ViewBag.Employees = await _employeeService.GetAllEmployeesAsync();
            ViewBag.SelectedDeptId = deptId;
            ViewBag.SelectedEmpId = empId;
            ViewBag.FromDate = from;
            ViewBag.ToDate = to;
            return View(attendances);
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

            TempData["SuccessMessage"] = "Attendance record created successfully!";
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

            TempData["SuccessMessage"] = "Attendance record updated successfully!";
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // POST: Attendance/UpdateStatus (AJAX)
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var (success, validation) = await _attendanceService.UpdateStatusAsync(id, status);
            
            if (!validation.IsValid)
            {
                var errorMessage = string.Join(", ", validation.Errors.Select(e => e.ErrorMessage));
                return Json(new { success = false, message = errorMessage });
            }

            return Json(new { success = true, message = "Status updated successfully!" });
        }

        // GET: Attendance/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _attendanceService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Attendance record deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
} 