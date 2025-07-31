using CodeZone.BLL.DTOs;
using CodeZone.BLL.Interfaces;
using CodeZone.DAL.Entities;
using CodeZone.DAL.Interface;
using CodeZone.DAL.Entities.Enum;
using FluentValidation;
using FluentValidation.Results;

namespace CodeZone.BLL.Services
{
    public class AttendanceService : BaseService, IAttendanceService
    {
        private readonly IAttendanceRepository _repo;
        private readonly IValidator<AttendanceDto> _validator;

        public AttendanceService(IAttendanceRepository repo, IValidator<AttendanceDto> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<Attendance?> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<AttendanceDto?> GetDtoByIdAsync(int id)
        {
            var attendance = await _repo.GetByIdAsync(id);
            if (attendance == null) return null;

            return new AttendanceDto
            {
                EmployeeId = attendance.EmployeeId,
                Date = attendance.Date,
                Status = attendance.Status
            };
        }

        public async Task<Attendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
            => await _repo.GetByEmployeeAndDateAsync(employeeId, date);

        public async Task<IEnumerable<Attendance>> GetAttendanceByEmployeeAsync(int employeeId)
        {
            var allAttendances = await _repo.GetAllAsync();
            return allAttendances.Where(a => a.EmployeeId == employeeId);
        }

        public async Task<(Attendance? attendance, ValidationResult validation)> AddAsync(AttendanceDto dto)
        {
            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return (null, validationResult);
            }

            // Check if attendance already exists for this employee on this date
            if (await _repo.ExistsAsync(dto.EmployeeId, dto.Date))
            {
                validationResult.Errors.Add(new ValidationFailure("Date", "Attendance has already been marked for this employee on this date."));
                return (null, validationResult);
            }

            var attendance = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                Date = dto.Date.Date,
                Status = dto.Status
            };

            await _repo.AddAsync(attendance);
            return (attendance, validationResult);
        }

        public async Task<(bool success, ValidationResult validation)> UpdateAsync(int id, AttendanceDto dto)
        {
            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return (false, validationResult);
            }

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                validationResult.Errors.Add(new ValidationFailure("Id", "Attendance record not found."));
                return (false, validationResult);
            }

            // Check if the status is the same (no change needed)
            if (entity.Status == dto.Status && entity.Date.Date == dto.Date.Date)
            {
                validationResult.Errors.Add(new ValidationFailure("Status", "No changes detected. The attendance status is already set to the same value."));
                return (false, validationResult);
            }

            // Check if attendance already exists for this employee on this date (excluding current record)
            if (await _repo.ExistsAsync(dto.EmployeeId, dto.Date, id))
            {
                validationResult.Errors.Add(new ValidationFailure("Date", "Attendance has already been marked for this employee on this date."));
                return (false, validationResult);
            }

            entity.Status = dto.Status;
            entity.Date = dto.Date.Date;

            await _repo.UpdateAsync(entity);
            return (true, validationResult);
        }

        public async Task<(bool success, ValidationResult validation)> UpdateStatusAsync(int id, string status)
        {
            var validationResult = new ValidationResult();

            // Get the attendance record
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                validationResult.Errors.Add(new ValidationFailure("Id", "Attendance record not found."));
                return (false, validationResult);
            }

            // Parse the status string to enum
            if (!Enum.TryParse<AttendanceStatus>(status, out var statusEnum))
            {
                validationResult.Errors.Add(new ValidationFailure("Status", "Invalid status value."));
                return (false, validationResult);
            }

            // Check if status is the same (no change needed)
            if (entity.Status == statusEnum)
            {
                validationResult.Errors.Add(new ValidationFailure("Status", "No changes detected. The attendance status is already set to the same value."));
                return (false, validationResult);
            }

            // Update only the status
            entity.Status = statusEnum;
            await _repo.UpdateAsync(entity);
            
            return (true, validationResult);
        }

        public async Task<(bool success, ValidationResult validation)> QuickUpdateAsync(int employeeId, DateTime date, string status)
        {
            var validationResult = new ValidationResult();

            try
            {
                // Parse the status string to enum
                if (!Enum.TryParse<AttendanceStatus>(status, out var statusEnum))
                {
                    validationResult.Errors.Add(new ValidationFailure("Status", "Invalid status value."));
                    return (false, validationResult);
                }

                // Create DTO from parameters (business logic)
                var attendanceDto = new AttendanceDto
                {
                    EmployeeId = employeeId,
                    Date = date,
                    Status = statusEnum
                };

                // Check if attendance record already exists for this employee and date
                var existingAttendance = await _repo.GetByEmployeeAndDateAsync(employeeId, date);
                
                if (existingAttendance != null)
                {
                    // Update existing record
                    return await UpdateAsync(existingAttendance.AttendanceId, attendanceDto);
                }
                else
                {
                    // Create new record
                    var (attendance, addValidation) = await AddAsync(attendanceDto);
                    return (attendance != null, addValidation);
                }
            }
            catch (Exception)
            {
                validationResult.Errors.Add(new ValidationFailure("General", "An error occurred while updating attendance."));
                return (false, validationResult);
            }
        }

        public async Task DeleteAsync(int id)
            => await _repo.DeleteAsync(id);

        public async Task<IEnumerable<Attendance>> FilterAsync(int? deptId, int? empId, DateTime? from, DateTime? to)
            => await _repo.FilterAsync(deptId, empId, from, to);

        public async Task<PaginationDto<Attendance>> GetAttendancesPaginatedAsync(int page = 1, int pageSize = 10)
        {
            var allAttendances = await _repo.GetAllAsync();
            var totalItems = allAttendances.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            
            // Ensure page is within valid range
            page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));
            
            var pagedAttendances = allAttendances
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            
            return new PaginationDto<Attendance>
            {
                Items = pagedAttendances,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<PaginationDto<Attendance>> GetFilteredAttendancesPaginatedAsync(int? deptId, int? empId, DateTime? from, DateTime? to, int page = 1, int pageSize = 10)
        {
            var allAttendances = await _repo.FilterAsync(deptId, empId, from, to);
            var totalItems = allAttendances.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            
            // Ensure page is within valid range
            page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));
            
            var pagedAttendances = allAttendances
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            
            return new PaginationDto<Attendance>
            {
                Items = pagedAttendances,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }
    }
}
