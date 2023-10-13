using InMemoryCacheDemo.Abstractions.PrimeNumbers;
using Microsoft.AspNetCore.Mvc;

namespace InMemoryCacheDemo.Controllers
{
    [ApiController]
    [Route("PrimeNumbers")]
    public class PrimeNumbersController : ControllerBase
    {
        private readonly IPrimeNumbersService _primeNumbersService;
        public PrimeNumbersController(IPrimeNumbersService primeNumbersService)
        {
            _primeNumbersService = primeNumbersService;
        }

        [HttpGet("{number}")]
        public async Task<string> Get(int number)
        {
            bool isItPrime = await _primeNumbersService.VerifyPrimeNumber(number);
            string message = $"{number} is {(isItPrime ? "Prime" : "Not Prime")}";
            return await Task.FromResult(message);
        }
    }
}