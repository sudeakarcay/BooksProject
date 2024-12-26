using BLL.DAL;
using System.ComponentModel;

namespace BLL.Models
{
    public class BookModel
    {
        public Book Record { get; set; }

        public string Name => Record.Name;

        [DisplayName("Number of pages")]
        public short NumberOfPages => Record.NumberOfPages ?? 0;

        [DisplayName("Publish Date")]
        public string PublishDate => Record.PublishDate.ToString("MM/dd/yyyy");

        public decimal Price => Record.Price;

        [DisplayName("Top Seller")]
        public string IsTopSeller => Record.IsTopSeller ? "Top Seller" : "Not Top Seller";

        [DisplayName("Author Name Surname")]
        public string Author => Record.Author != null ? string.Join(" ", Record.Author.Name, Record.Author.Surname) : "Unknown Author";

        public string Genres => string.Join("<br>", Record.BookGenres?.Select(bg => bg.Genre?.Name));

        [DisplayName("Genres")]
        public List<int> GenreIds
        {
            get => Record.BookGenres?.Select(bg => bg.GenreId).ToList();

            set => Record.BookGenres = value.Select(v => new BookGenre() { GenreId = v }).ToList();
        }

    }
}
