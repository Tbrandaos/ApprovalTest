using ApprovalTest.Domain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAPI.Models;
using System;
using System.Threading.Tasks;

namespace ApprovalTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsApiController : ControllerBase
    {
        private readonly INewsService _service;

        public NewsApiController(INewsService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("/GetEverything")]
        public async Task<IActionResult> GetEverything([FromQuery] EverythingRequest request)
        {
            try
            {
                var result = await _service.GetEverything(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("/GetTopHeadlines")]
        public async Task<IActionResult> GetTopHeadlines([FromQuery] TopHeadlinesRequest request)
        {
            try
            {
                var result = await _service.GetTopHeadlines(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
