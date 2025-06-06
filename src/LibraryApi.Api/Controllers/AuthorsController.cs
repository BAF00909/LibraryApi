using LibraryApi.Application.Dtos;
using LibraryApi.Application.Services;
using LibraryApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorsController> _logger;
        public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorDto>>> GetAll()
        {
            _logger.LogInformation("Получение всех авторов");
            var authors = await _authorService.GetAllAsync();
            _logger.LogInformation("Возвращено {AuthorCounter} авторов", authors.Count);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetById(int id)
        {
            _logger.LogInformation("Получение автора с ID {AuthorId}", id);
            var author = await _authorService.GetByIdAsync(id);
            if(author == null)
            {
                _logger.LogWarning("Автор с ID {AuthorId} не найден", id);
                return NotFound();
            }
            return  Ok(author);
        }

        // POST api/<AuthorsController>
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> Create([FromBody]CreateAuthorDto createAutor)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Некоректные данные для создания автора {Error}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }
            try
            {
                var newAuthor = await _authorService.CreateAsync(createAutor);
                return CreatedAtAction(nameof(GetById), new { id = newAuthor.Id }, newAuthor);
            } catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка при создании автора");
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<AuthorsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorDto>> Update(int id, [FromBody]UpdateAuthorDto updateAuthorDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Некоректные данные для обновления автора {Error}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }
            try
            {
                var author = await _authorService.UpdateAsync(id, updateAuthorDto);
                if(author == null)
                {
                    _logger.LogWarning("Автор с ID {AuthorId} не найден", id);
                    return NotFound();
                }
                return Ok(author);
            }catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка при обновлении автора");
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _authorService.DeleteAsync(id);
            if(!deleted)
            {
                _logger.LogWarning("Автор с ID {AuthorId} не найден", id);
                return NotFound();
            }
            return NoContent();
        }
    }
}
