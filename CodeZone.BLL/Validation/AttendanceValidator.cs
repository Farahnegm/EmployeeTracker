using FluentValidation;
using CodeZone.DAL.Entities;
using CodeZone.DAL.Data;
using System;
using System.Linq;

namespace CodeZone.BLL.Validation
{
    public class AttendanceValidator : AbstractValidator<Attendance>
    {
        public AttendanceValidator(AppDbContext db)
        {
            RuleFor(a => a.EmployeeId)
                .NotEmpty();

            RuleFor(a => a.Date)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Attendance date cannot be in the future.");

            RuleFor(a => a)
                .Must(a => !db.Attendances.Any(att => att.EmployeeId == a.EmployeeId && att.Date == a.Date && att.AttendanceId != a.AttendanceId))
                .WithMessage("Attendance for this employee on this date already exists.");

            RuleFor(a => a.Status)
                .IsInEnum().WithMessage("Status must be Present or Absent.");
        }
    }
} 