using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Application.Dtos
{
    public class CreateAuthorDto
    {
        [Required(ErrorMessage = "Имя Автора Обязательно")]
        [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
        public string Name { get; set; } = string.Empty;
        public DateTime? BirthDay { get; set; }
    }
}
