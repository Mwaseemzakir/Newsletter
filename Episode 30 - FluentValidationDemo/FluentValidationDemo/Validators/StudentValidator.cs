using FluentValidation;
using FluentValidationDemo.DTOs;

namespace FluentValidationDemo.Validators
{
    public class StudentValidator : AbstractValidator<StudentDto>
    {
        public StudentValidator()
        {
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.PhoneNumber)
                .Length(11);

            RuleFor(x => x.Age)
                .GreaterThanOrEqualTo(18);

        }
    }
}
