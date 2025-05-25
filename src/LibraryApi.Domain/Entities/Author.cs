using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Domain.Entities
{
    public record Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? BirthDay { get; set; }
        public List<Book> Books { get; set; } = new();
    }
}
