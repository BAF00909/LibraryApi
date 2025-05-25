using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Domain.Entities
{
    public record Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public int Pages { get; set; }
        public Genre Genre { get; set; }
        public Author Author { get; set; } = new();
        public List<Loan> Loans { get; set; } = new();
    }
}
