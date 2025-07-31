using FluentValidation;
using CodeZone.BLL.DTOs;
using System;
using System.Diagnostics;

namespace CodeZone.BLL.Validation
{
    public class AttendanceDtoValidator : AbstractValidator<AttendanceDto>
    {
        public AttendanceDtoValidator()
        {
            RuleFor(a => a.EmployeeId)
                .NotEmpty().WithMessage("Employee must be selected.")
                .GreaterThan(0).WithMessage("Please select a valid employee.");

            RuleFor(a => a.Date)
                .NotEmpty().WithMessage("Date is required.")
                .Must(date => 
                {
                    var inputDate = date.Date;
                    var todayDate = DateTime.Today.Date;
                    var isValid = inputDate <= todayDate;
                    
                    return isValid;
                })
                .WithMessage($"Attendance date cannot be in the future. Today's date is {DateTime.Today:MM/dd/yyyy}.");

            RuleFor(a => a.Status)
                .IsInEnum().WithMessage("Status must be Present or Absent.");
        }
    }
}