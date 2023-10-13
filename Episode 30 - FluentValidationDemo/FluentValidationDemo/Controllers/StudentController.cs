using FluentValidation;
using FluentValidationDemo.DTOs;
using FluentValidationDemo.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FluentValidationDemo.Controllers
{
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        private readonly IValidator<StudentDto> _validator;

        private static List<StudentDto> students = new();
        public StudentController(IValidator<StudentDto> validator)
        {
            _validator = validator;
        }

        [HttpGet]
        public List<StudentDto> Get()
        {
            return students;
        }

        [HttpPost]
        public string Add(StudentDto student)
        {
            var validate = _validator.Validate(student);

            if (validate.IsValid)
            {
                students.Add(student);

                return "Student added succssfully";
            }

            return validate.Errors.Get();
        }
    }
}
