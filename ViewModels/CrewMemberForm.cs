using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppMVC1.Models;

namespace WebAppMVC1.ViewModels {
    public class CrewMemberForm {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Select a role.")]
        [EnumDataType(typeof(CrewRole))]
        public CrewRole? Role { get; set; }

        [Required(ErrorMessage = "Select a crew.")]
        [Display(Name = "Crew")]
        public int? RocketCrewId { get; set; }

        public List<SelectListItem> Crews { get; set; } = new();
    }
}
