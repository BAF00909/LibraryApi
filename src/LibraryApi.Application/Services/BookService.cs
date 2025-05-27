using LibraryApi.Application.Dtos;
using LibraryApi.Domain.Repositories;
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
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<List<BookDto>> GetAllAsync()
        {
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
