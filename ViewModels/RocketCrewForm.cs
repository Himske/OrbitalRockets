using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAppMVC1.ViewModels {
    public class RocketCrewForm {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Crew name")]
        public string CrewName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Select a rocket.")]
        [Display(Name = "Rocket")]
        public int? RocketId { get; set; }

        public List<SelectListItem> Rockets { get; set; } = new();
    }
}
