namespace WebAppMVC1.Models {
    public class RocketCrew {
        public int Id { get; set; }
        public required string CrewName { get; set; }

        public int RocketId { get; set; }
        public Rocket? Rocket { get; set; }
        public ICollection<CrewMember> Members { get; set; } = new List<CrewMember>();
    }
}
