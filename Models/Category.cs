using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class Category 
    {
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
