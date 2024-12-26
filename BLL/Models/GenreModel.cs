using BLL.DAL;
using System.ComponentModel;

namespace BLL.Models
{
    public class GenreModel
    {
        public Genre Record { get; set; }

        [DisplayName("Genre Name")]
        public string Name => Record.Name;

    }
}
