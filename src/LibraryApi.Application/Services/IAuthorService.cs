using LibraryApi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Application.Services
{
    interface IAuthorService
    {
        Task<List<AuthorDto>> GetAllAsync();
        Task<AuthorDto?> GetByIdAsync(int id);
        Task<AuthorDto> CreateAsync(CreateAuthorDto createAuthorDto);
        Task<AuthorDto?> UpdateAsync(int id, UpdateAuthorDto updateAuthorDto);
        Task<bool> DeleteAsync(int id);
    }
}
