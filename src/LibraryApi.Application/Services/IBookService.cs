using LibraryApi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Application.Services
{
    public interface IBookService
    {
        public Task<List<BookDto>> GetAllAsync();
    }
}
