using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumia.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string FullName { get; set; }
        public string Professian { get; set; }
        [Required]
        public string Bio { get; set; }
        public string? Img { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }

        [NotMapped]
        public IFormFile FormFile { get; set; }

    }
}
