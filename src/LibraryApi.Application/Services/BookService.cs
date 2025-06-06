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
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<BookService> _logger;
        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _logger = logger;
        }
        public async Task<List<BookDto>> GetAllAsync()
        {
            _logger.LogInformation("Получаем все книги");
            var books = await _bookRepository.GetAllAsync();
            return books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                PublicationYear = b.PublicationYear,
                Pages = b.Pages,
                Genre = b.Genre,
                AuthorNames = b.Authors.Select(a => a.Name).ToList()
            }).ToList();
        }
        public async Task<BookDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Получаем книгу с ID {BookId}", id);
            var book = await _bookRepository.GetByIdAsyn(id);
            if(book == null)
            {
                _logger.LogWarning("Книга с ID {BookID} не найдена", id);
                return null;
            }
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                AuthorNames = book.Authors.Select(a => a.Name).ToList(),
                Pages = book.Pages,
                Genre = book.Genre,
                PublicationYear = book.PublicationYear
            };
            return bookDto;
        }
        public async Task<BookDto> CreateAsync(CreateBookDto createBookDto)
        {
            _logger.LogInformation("Создаем книгу с наименованием {BookName}", createBookDto.Title);
            var authors = await _authorRepository.GetAllAsync();
            var selectedAuthors = authors.Where(a => createBookDto.AuthorIds.Contains(a.Id)).ToList();
            if(selectedAuthors.Count != createBookDto.AuthorIds?.Count)
            {
                _logger.LogWarning("Некоторые авторы не найдены: {Authorids}", string.Join(",", createBookDto.AuthorIds));
                throw new ArgumentException("Один или несколько авторов не найдены");
            }
            var newBook = new Book
            {
                Title = createBookDto.Title,
                Authors = selectedAuthors,
                Genre=createBookDto.Genre,
                Pages = createBookDto.Pages,
                PublicationYear=createBookDto.PublicationYear
            };
            await _bookRepository.AddAsync(newBook);
            var bookDto = new BookDto
            {
                Id = newBook.Id,
                Title = newBook.Title,
                Pages = newBook.Pages,
                Genre = newBook.Genre,
                PublicationYear = newBook.PublicationYear,
                AuthorNames = newBook.Authors.Select(a => a.Name).ToList()
            };
            _logger.LogInformation("Книга с наименованием {BookName} создана", createBookDto.Title);
            return bookDto;
        }
        public async Task<BookDto?> UpdateAsync(int id, UpdateBookDto updateBookDto)
        {
            _logger.LogInformation("Обнавление книги с ID {BookId}", id);
            var book = await _bookRepository.GetByIdAsyn(id);
            if(book == null)
            {
                _logger.LogWarning("Книга с ID {BookId} не найдена", id);
                return null;
            }
            if(updateBookDto.Title != null)
            {
                book.Title = updateBookDto.Title;
            }
            if (updateBookDto.Genre.HasValue)
            {
                book.Genre = updateBookDto.Genre.Value;
            }
            if(updateBookDto.PublicationYear.HasValue)
            {
                book.PublicationYear = updateBookDto.PublicationYear.Value;
            }
            if(updateBookDto.Pages.HasValue)
            {
                book.Pages = updateBookDto.Pages.Value;
            }
            if(updateBookDto.AuthorIds != null)
            {
                var authors = await _authorRepository.GetAllAsync();
                var selectedAuthors = authors.Where(a => updateBookDto.AuthorIds.Contains(a.Id)).ToList();
                if (selectedAuthors.Count != updateBookDto.AuthorIds.Count)
                {
                    _logger.LogWarning("Некоторые авторы не найдены: {Authorids}", string.Join(",", updateBookDto.AuthorIds));
                    throw new ArgumentException("Один или несколько авторов не найдены");
                }
                book.Authors = selectedAuthors;
            }
            await _bookRepository.UpdateAsync(book);
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Genre = book.Genre,
                PublicationYear = book.PublicationYear,
                Pages = book.Pages,
                AuthorNames = book.Authors.Select(a => a.Name).ToList()
            };
            _logger.LogInformation("Книга с ID {BookId} обновлена", id);
            return bookDto;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Удаляем книгу с ID {BookID}", id);
            var book = await _bookRepository.GetByIdAsyn(id);
            if (book == null)
            {
                _logger.LogWarning("Книга с ID {BookId} не найдена", id);
                return false;
            }
            await _bookRepository.DeleteAsync(id);
            _logger.LogInformation("Книга с ID {BookID} удалена", id);
            return true;
        }
    }
}
