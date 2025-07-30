using Microsoft.AspNetCore.Mvc;
using CodeZone.BLL.Interfaces;
using CodeZone.DAL.Entities;

namespace CodeZone_Task.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: Department
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            return View(departments);
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var department = await _departmentService.GetDepartmentAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Code,Location")] Department department)
        {
            var (createdDepartment, validation) = await _departmentService.CreateAsync(department);
            
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(department);
            }

            TempData["SuccessMessage"] = _departmentService.GetSuccessMessage("created", "Department");
            return RedirectToAction(nameof(Details), new { id = createdDepartment!.DepartmentId });
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetDepartmentAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,Name,Code,Location")] Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            var (success, validation) = await _departmentService.UpdateAsync(department);
            
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(department);
            }

            TempData["SuccessMessage"] = _departmentService.GetSuccessMessage("updated", "Department");
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetDepartmentAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (success, validation) = await _departmentService.DeleteAsync(id);
            
            if (!validation.IsValid)
            {
                TempData["ErrorMessage"] = _departmentService.GetValidationErrorMessage(validation);
            }
            else
            {
                TempData["SuccessMessage"] = _departmentService.GetSuccessMessage("deleted", "Department");
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
} 