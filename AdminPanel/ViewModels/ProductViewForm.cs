using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;

namespace AdminPanel.ViewModels
{
    public class ProductViewForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public IFormFile? Image { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        [Required]
        public int ProductBrandId { get; set; }
        //public ProductBrand ProductBrand { get; set; }
        [Required]
        public int ProductTypeId { get; set; }
        //public ProductType ProductType { get; set; }
    }
}
