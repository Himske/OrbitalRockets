using Microsoft.EntityFrameworkCore;
using WebAppMVC1.Models;

namespace WebAppMVC1 {
    public class WebAppMVCContext(DbContextOptions<WebAppMVCContext> options) : DbContext(options) {
        public DbSet<Rocket> Rockets { get; set; }
        public DbSet<RocketCrew> RocketCrews { get; set; }
        public DbSet<CrewMember> CrewMembers { get; set; }
    }
}
