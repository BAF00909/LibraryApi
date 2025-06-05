using LibraryApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Application.Dtos
{
    public class CreateBookDto
    {
        [Required(ErrorMessage = "Название книги обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string Title { get; set; } = string.Empty;
        [Range(1, 9999, ErrorMessage = "Год публикации должен быть между 1 и 9999")]
        public int PublicationYear { get; set; }
        [Range(1, 10000, ErrorMessage = "Количество страниц должно быть от 1 до 10000")]
        public int Pages { get; set; }
        [Required(ErrorMessage = "Жанр обязателен")]
        public Genre Genre { get; set; }
        [Required(ErrorMessage = "Нужно указать хотябы одного автора")]
        public List<int> AuthorIds { get; set; } = new();
    }
}
