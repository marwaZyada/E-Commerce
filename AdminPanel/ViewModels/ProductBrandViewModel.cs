using Microsoft.EntityFrameworkCore;

namespace AdminPanel.ViewModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class ProductBrandViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}
