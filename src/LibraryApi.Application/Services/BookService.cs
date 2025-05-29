using LibraryApi.Application.Dtos;
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
        private readonly ILogger<BookService> _logger;
        public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }
        public async Task<List<BookDto>> GetAllAsync()
        {
            _logger.LogInformation("Get all books fromn repository");
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
    }
}
