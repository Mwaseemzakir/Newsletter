using FluentValidation.Results;
using System.Text;

namespace FluentValidationSample.Extensions
{
    public static class ErrorBuilder
    {
        public static string Get(this List<ValidationFailure> errors)
        {
            StringBuilder errorBuilder = new();
            foreach (var error in errors)
            {
                errorBuilder.Append($"{error} \n");
            }
            return errorBuilder.ToString();
        }
    }
}
