using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.BlazorUI.ViewModels.LeaveTypes
{
    public class LeaveTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [Display(Name = "Default Number of Days")]
        public int DefaultDays { get; set; }
    }
}
