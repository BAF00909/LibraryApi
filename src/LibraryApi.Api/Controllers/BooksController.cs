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
            _logger.LogInformation("Получение всех книг");
            var books = await _bookService.GetAllAsync();
            _logger.LogInformation($"Возвращено {books.Count} книг");
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            _logger.LogInformation("Получение книги ID {BookId}", id);
            var book = await _bookService.GetByIdAsync(id);
            if(book == null)
            {
                _logger.LogWarning("Книга с ID {BookId} не найдена", id);
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto createBook)
        {
            _logger.LogInformation("Создание книги с наименованием {Title}", createBook.Title);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Некоректные данные для создания книги {Error}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }
            try
            {
                var book = await _bookService.CreateAsync(createBook);
                _logger.LogInformation("Книга с наименованием {Title} создана", createBook.Title);
                return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
            }catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка при создании книги");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> Update(int id, [FromBody] UpdateBookDto updateBook)
        {
            _logger.LogInformation("Обновление книги ID {BookId}", id);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Некоректные данные для обновления книги {Error}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }
            try
            {
                var book = await _bookService.UpdateAsync(id, updateBook);
                if(book == null)
                {
                    _logger.LogWarning("Книга с ID {BookId} не найдена", id);
                    return NotFound();
                }
                _logger.LogInformation("Книга с ID {BookId} обновлена", id);
                return Ok(book);
            } catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка при обновлении книги");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Удаление книги ID {BookId}", id);
            var deleted = await _bookService.DeleteAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Книга с ID {BookId} не найдена", id);
                return NotFound();
            }
            return NoContent();
        }
    }
}
