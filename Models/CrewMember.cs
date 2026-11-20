using System.ComponentModel.DataAnnotations;

namespace WebAppMVC1.Models {
    public class CrewMember {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        [EnumDataType(typeof(CrewRole))]
        public CrewRole Role { get; set; }

        public int RocketCrewId { get; set; }
        public RocketCrew? RocketCrew { get; set; }
    }
}
