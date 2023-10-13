using DapperDemo.Contracts;
using DapperDemo.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DapperDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsletterController : ControllerBase
    {
        private readonly INewsletterRepository _newsletterRepository;
        public NewsletterController(INewsletterRepository newsletterRepository)
        {
            _newsletterRepository = newsletterRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var newsletters = await _newsletterRepository.GetAll();
            return Ok(newsletters);
        }
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] NewsletterDTO newsletterDTO)
        {
            var isSuccessful = await _newsletterRepository.Create(newsletterDTO);
            return Ok(isSuccessful);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var newsletter = await _newsletterRepository.GetById(Id);
            return Ok(newsletter);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var newsletter = await _newsletterRepository.GetById(Id);
            if (newsletter is null) return NotFound();
            var isDeleted = await _newsletterRepository.Delete(Id);
            return Ok(isDeleted);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, NewsletterDTO newsletterDto)
        {
            var newsletter = await _newsletterRepository.GetById(Id);
            if (newsletter is null) return NotFound();
            var isUpdated = await _newsletterRepository.Update(Id, newsletterDto);
            return Ok(isUpdated);
        }
    }
}
