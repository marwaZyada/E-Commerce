namespace AdminPanel.ViewModels
{
    public class UserFormViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleViewModel> Roles{ get; set; }
    }
}
