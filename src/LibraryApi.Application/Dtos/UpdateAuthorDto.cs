using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Application.Dtos
{
    public class UpdateAuthorDto
    {
        [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
        public string? Name { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}
