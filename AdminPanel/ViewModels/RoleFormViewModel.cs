using System.ComponentModel.DataAnnotations;

namespace AdminPanel.ViewModels
{
    public class RoleFormViewModel
    {
      
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
    }
}
