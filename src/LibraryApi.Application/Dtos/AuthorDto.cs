using LibraryApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Application.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? BirthDay { get; set; }
        public List<string> BookTitles { get; set; } = new();
    }
}
