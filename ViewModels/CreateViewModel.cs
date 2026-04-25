using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Website.ViewModels
{
    public class CreateViewModel
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Prix en dinar :")]
        public float Price { get; set; }

        [Required]
        [Display(Name = "Quantité en unité :")]
        public int QteStock { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public int? CategoryId { get; set; }

        [Display(Name = "Image :")]
        public IFormFile ImagePath { get; set; }  
    }
}