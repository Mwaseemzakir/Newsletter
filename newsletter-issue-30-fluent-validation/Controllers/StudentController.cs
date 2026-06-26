using FluentValidation;
using FluentValidation.DTOs;
using FluentValidation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FluentValidation.Controllers
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
