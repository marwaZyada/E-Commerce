using System.ComponentModel.DataAnnotations;

namespace AdminPanel.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
