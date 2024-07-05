using System.ComponentModel.DataAnnotations;
namespace CoreMVCProject.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        [Required]
        public string BookName { get; set; }
        public string BookDescription { get; set; }
        public string BookCategory { get; set; }
        public IFormFile? ImageFileName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double Quantity { get; set; }
    }
}
