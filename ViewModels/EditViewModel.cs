using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Website.ViewModels
{
    public class EditViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Prix en dinar :")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Display(Name = "Quantité en unité :")]
        public int QteStock { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        [Display(Name = "Image :")]
        public IFormFile ImagePath { get; set; }

        public string ExistingImagePath { get; set; }
    }
}