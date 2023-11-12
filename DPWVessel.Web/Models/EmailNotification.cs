using System.ComponentModel.DataAnnotations;

namespace DPWVessel.Web.Models
{
    public class EmailNotification
    {
        [Required]
        public int TicketId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int DeptId { get; set; }
        public string DeptEmail { get; set; }
        public string OrganizationLogo { get; set; }
        public string url { get; set; }
    }
}