using System.ComponentModel.DataAnnotations;

namespace Website.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<UserRoleViewModel>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public List<UserRoleViewModel> Users { get; set; }
    }
}