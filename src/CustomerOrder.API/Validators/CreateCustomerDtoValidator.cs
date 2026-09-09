using CustomerOrder.Domain.DTOs;
using FluentValidation;

namespace CustomerOrder.API.Validators;

public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("A valid email address is required.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(16).WithMessage("Phone number cannot exceed 16 characters.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}