namespace WebAppMVC1.Models {
    public class Rocket {
        public int Id { get; set; }
        public required string RocketName { get; set; }
        public required string Manufacturer { get; set; }
        public ICollection<RocketCrew> Crews { get; set; } = new List<RocketCrew>();
    }
}
