using LibraryApi.Application.Dtos;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Application.Services
{
    public class AuthorService: IAuthorService
    {
        private readonly IAuthorRepository _repository;
        private readonly ILogger<AuthorService> _logger;
        public AuthorService(IAuthorRepository repository, ILogger<AuthorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<List<AuthorDto>> GetAllAsync()
        {
            _logger.LogInformation("Извлечение всех авторов");
            var authors = await _repository.GetAllAsync();
            var authorDtos = authors.Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name,
                BirthDay = a.BirthDay,
                BookTitles = a.Books.Select(b => b.Title).ToList()
            }).ToList();
            _logger.LogInformation("Получено {AuthorCount} авторов", authorDtos.Count);
            return authorDtos;
        }
        public async Task<AuthorDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Извлечение автора с Id {AuthorID}", id);
            var author = await _repository.GetByIdAsync(id);
            if(author == null)
            {
                _logger.LogWarning("Автор с Id {AuthodID} не найден", id);
                return null;
            }
            var authorDto = new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                BirthDay = author.BirthDay,
                BookTitles = author.Books.Select(b => b.Title).ToList(),
            };
            _logger.LogInformation("Автор с Id: {AuthorID} получен", id);
            return authorDto;
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorDto createAuthorDto)
        {
            _logger.LogInformation("Создание нового автора: {AuthorName}", createAuthorDto.Name);
            var author = new Author
            {
                Name = createAuthorDto.Name,
                BirthDay = createAuthorDto.BirthDay,
            };
            await _repository.AddAsync(author);
            var autorDto = new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                BirthDay = author.BirthDay,
                BookTitles = new()
            };
            _logger.LogInformation("Создан новый автор с ID {AuthorID}", author.Id);
            return autorDto;
        }
        public async Task<AuthorDto?> UpdateAsync(int id, UpdateAuthorDto updateAuthorDto)
        {
            _logger.LogInformation("Обновление автора с ID {AuthorID}", id);
            var author = await _repository.GetByIdAsync(id);
            if(author == null)
            {
                _logger.LogWarning("Автор с Id {AuthodID} не найден", id);
                return null;
            }
            if(updateAuthorDto.Name != null)
            {
                author.Name = updateAuthorDto.Name;
            }
            if(updateAuthorDto.BirthDay != null)
            {
                author.BirthDay = updateAuthorDto.BirthDay;
            }
            await _repository.UpdateAsync(author);
            var authorDto = new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                BirthDay = author.BirthDay,
                BookTitles = author.Books.Select(book => book.Title).ToList()
            };
            _logger.LogInformation("Автор с ID {AuthorID} обновлен", id);
            return authorDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Удаление автора с ID {AuthorID}", id);
            var author = await _repository.GetByIdAsync(id);
            if(author == null)
            {
                _logger.LogWarning("Автор с Id {AuthodID} не найден", id);
                return false;
            }
            await _repository.DeleteAsync(id);
            _logger.LogInformation("Автор с ID {AuthorID} удален", id);
            return true;
        }
    }
}
