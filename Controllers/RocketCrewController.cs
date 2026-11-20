using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppMVC1.Models;
using WebAppMVC1.ViewModels;

namespace WebAppMVC1.Controllers {
    public class RocketCrewController(WebAppMVCContext context) : Controller {
        public async Task<IActionResult> Index() {
            var crews = await context.RocketCrews.AsNoTracking().Include(c => c.Rocket).ToListAsync();
            return View(crews);
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) return NotFound();
            var crew = await context.RocketCrews.AsNoTracking().Include(c => c.Rocket)
                .Include(c => c.Members).FirstOrDefaultAsync(c => c.Id == id);
            return crew == null ? NotFound() : View(crew);
        }

        public async Task<IActionResult> Create() {
            var form = new RocketCrewForm();
            await PopulateRockets(form);
            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CrewName,RocketId")] RocketCrewForm form) {
            await ValidateRocket(form);
            if (!ModelState.IsValid) {
                await PopulateRockets(form);
                return View(form);
            }

            context.RocketCrews.Add(new RocketCrew { CrewName = form.CrewName, RocketId = form.RocketId!.Value });
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();
            var crew = await context.RocketCrews.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (crew == null) return NotFound();
            var form = new RocketCrewForm { Id = crew.Id, CrewName = crew.CrewName, RocketId = crew.RocketId };
            await PopulateRockets(form);
            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CrewName,RocketId")] RocketCrewForm form) {
            var crew = await context.RocketCrews.FindAsync(id);
            if (crew == null) return NotFound();
            await ValidateRocket(form);
            if (!ModelState.IsValid) {
                form.Id = id;
                await PopulateRockets(form);
                return View(form);
            }

            crew.CrewName = form.CrewName;
            crew.RocketId = form.RocketId!.Value;
            try {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!await context.RocketCrews.AnyAsync(c => c.Id == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return NotFound();
            var crew = await context.RocketCrews.AsNoTracking().Include(c => c.Rocket)
                .FirstOrDefaultAsync(c => c.Id == id);
            return crew == null ? NotFound() : View(crew);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var crew = await context.RocketCrews.FindAsync(id);
            if (crew == null) return NotFound();
            context.RocketCrews.Remove(crew);
            try {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!await context.RocketCrews.AnyAsync(c => c.Id == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task ValidateRocket(RocketCrewForm form) {
            if (form.RocketId.HasValue && !await context.Rockets.AnyAsync(r => r.Id == form.RocketId.Value)) {
                ModelState.AddModelError(nameof(form.RocketId), "Select an existing rocket.");
            }
        }

        private async Task PopulateRockets(RocketCrewForm form) {
            form.Rockets = await context.Rockets.AsNoTracking().OrderBy(r => r.RocketName)
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.RocketName }).ToListAsync();
        }
    }
}
