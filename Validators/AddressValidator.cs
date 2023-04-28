using System;
using FluentValidation;
using Mixed.Models;
namespace Mixed.Validators
{
	public class AddressValidator:AbstractValidator<Address>
	{
        public AddressValidator()
        {
            RuleFor(x => x.Country.ToLower())
                .NotEmpty().WithMessage("Country cannot be empty");
            RuleFor(x => x.City.ToLower())
                .NotEmpty().WithMessage("City cannot be empty");
            RuleFor(x => x.Homenumber)
                .NotEmpty().WithMessage("Home number cannot be empty")
                .LessThan(999).WithMessage("Home number cannot be larger than 999")
                .GreaterThan(-1).WithMessage("Home number cannot be negative number");
        }

    }
}

