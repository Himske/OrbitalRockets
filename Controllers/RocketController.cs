using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppMVC1.Models;

namespace WebAppMVC1.Controllers {
    public class RocketController(WebAppMVCContext context) : Controller {
        public async Task<IActionResult> Index() {
            return View(await context.Rockets.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var rocket = await context.Rockets.AsNoTracking().Include(r => r.Crews)
                .FirstOrDefaultAsync(r => r.Id == id);
            return rocket == null ? NotFound() : View(rocket);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RocketName,Manufacturer")] Rocket rocket) {
            if (!ModelState.IsValid) {
                return View(rocket);
            }

            context.Rockets.Add(rocket);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var rocket = await context.Rockets.FindAsync(id);
            return rocket == null ? NotFound() : View(rocket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RocketName,Manufacturer")] Rocket rocket) {
            if (id != rocket.Id) {
                return NotFound();
            }

            if (!ModelState.IsValid) {
                return View(rocket);
            }

            var existingRocket = await context.Rockets.FindAsync(id);
            if (existingRocket == null) {
                return NotFound();
            }

            existingRocket.RocketName = rocket.RocketName;
            existingRocket.Manufacturer = rocket.Manufacturer;
            try {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!await context.Rockets.AnyAsync(r => r.Id == id)) {
                    return NotFound();
                }
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var rocket = await context.Rockets.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            return rocket == null ? NotFound() : View(rocket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var rocket = await context.Rockets.FindAsync(id);
            if (rocket == null) {
                return NotFound();
            }

            context.Rockets.Remove(rocket);
            try {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!await context.Rockets.AnyAsync(r => r.Id == id)) {
                    return NotFound();
                }
                throw;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
