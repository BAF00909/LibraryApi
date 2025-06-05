using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Infrastructure
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;
        public BookRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.AsNoTracking().Include(b => b.Authors).ToListAsync();
        }
        public async Task<Book?> GetByIdAsyn(int id)
        {
            return await _context.Books.AsNoTracking().Include(b => b.Authors).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Book author)
        {
            await _context.Books.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if(book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}
