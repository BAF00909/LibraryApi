using Microsoft.AspNetCore.Mvc;
using LibraryApi.Application.Services;
using System.Threading.Tasks;
using LibraryApi.Application.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;
        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetAll()
        {
            _logger.LogInformation("Get all books");
            var books = await _bookService.GetAllAsync();
            _logger.LogInformation($"Return {books.Count} books");
            return Ok(books);
        }
    }
}
