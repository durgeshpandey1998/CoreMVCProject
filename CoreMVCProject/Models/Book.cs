using System.ComponentModel.DataAnnotations;

namespace CoreMVCProject.Models
{
    public class Book
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string BookName { get; set; }
        public string BookDescription { get; set; }
        public string BookCategory { get; set; }
        public string? ImageFileName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double Quantity { get; set; }
    }
}
