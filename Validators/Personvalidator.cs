using System;
using FluentValidation;
using Mixed.Models;
namespace Mixed.Validators
{
	public class Personvalidator:AbstractValidator<Person>
	{
		public Personvalidator()
		{
			RuleFor(x=>x.CreateDate).NotEmpty().
				WithMessage("createdate is required")
				.LessThanOrEqualTo(DateTime.Today).WithMessage($"Create date cannot be greater than {DateTime.Today.Date}");
            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(50).WithMessage("First Name is too long, enter less than 50 letters");
            RuleFor(x => x.Lastname)
                    .NotEmpty().WithMessage("Last Name is required")
                    .MaximumLength(50).WithMessage("Last Name is too long, enter less than 50 letters");
            RuleFor(x => x.Jobposition)
                    .NotEmpty().WithMessage("Position is required")
                    .MaximumLength(50).WithMessage("Position name is too long, enter less than 50 letters");
            RuleFor(x => x.Salary)
                    .NotNull().WithMessage("Salary cannot be empty")
                    .InclusiveBetween(0, 10000).WithMessage("Incorrect salary amount, enter between 0-10.000");
            RuleFor(x => x.Workexperience)
                    .NotEmpty().WithMessage("Work experience is required")
                    .GreaterThan(-1).WithMessage("Work experience cannot be nagative number")
                    .LessThan(80).WithMessage("Work experience number is invalid");

            RuleFor(x => x.PersonAddress).SetValidator(new AddressValidator());
        }
		
	}
}

