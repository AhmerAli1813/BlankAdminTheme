using System.ComponentModel.DataAnnotations;

namespace DPWVessel.Web.Models
{
    public class UserInfo
    {
        [Required(ErrorMessageResourceType = typeof(Model.Resources.Messages), ErrorMessageResourceName = "UsernameRequired")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Model.Resources.Messages), ErrorMessageResourceName = "FullNameRequired")]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Model.Resources.Messages), ErrorMessageResourceName = "FullNameArRequired")]
        [Display(Name = "FullNameAr")]
        public string FullNameAr { get; set; }

        [Required(ErrorMessageResourceType = typeof(Model.Resources.Messages), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string UserImage { get; set; }

        [Required(ErrorMessageResourceType = typeof(Model.Resources.Messages), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(Model.Resources.Messages), ErrorMessageResourceName = "Passwordlength", MinimumLength = 6)]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessageResourceType = typeof(Model.Resources.Messages), ErrorMessageResourceName = "Passwordnotmatched")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Model.Resources.Messages), ErrorMessageResourceName = "UserTypeRequired")]
        public int UserType { get; set; }

        public string EmpId { get; set; }

        public int SupplierId { get; set; }

        public int[] LocationId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Model.Resources.Messages), ErrorMessageResourceName = "PhoneRequired")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        public int OrganizationId { get; set; }

        public int DepartmentId { get; set; }
        public bool WebAccess { get; set; }


    }
}