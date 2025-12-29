using System.ComponentModel.DataAnnotations;


namespace DevHive.Web.Models.ViewModels
{
    public class MyAccountViewModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new();

        // שינוי סיסמה בתוך אותו עמוד
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm new password")]
        public string? ConfirmNewPassword { get; set; }
    }
}
