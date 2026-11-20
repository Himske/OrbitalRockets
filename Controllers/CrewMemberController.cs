using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppMVC1.Models;
using WebAppMVC1.ViewModels;

namespace WebAppMVC1.Controllers {
    public class CrewMemberController(WebAppMVCContext context) : Controller {
        public async Task<IActionResult> Index() {
            var members = await context.CrewMembers.AsNoTracking().Include(m => m.RocketCrew)
                .ThenInclude(c => c!.Rocket).ToListAsync();
            return View(members);
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) return NotFound();
            var member = await context.CrewMembers.AsNoTracking().Include(m => m.RocketCrew)
                .ThenInclude(c => c!.Rocket).FirstOrDefaultAsync(m => m.Id == id);
            return member == null ? NotFound() : View(member);
        }

        public async Task<IActionResult> Create() {
            var form = new CrewMemberForm();
            await PopulateCrews(form);
            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Role,RocketCrewId")] CrewMemberForm form) {
            await ValidateCrew(form);
            if (!ModelState.IsValid) {
                await PopulateCrews(form);
                return View(form);
            }

            context.CrewMembers.Add(new CrewMember {
                FirstName = form.FirstName, LastName = form.LastName,
                Role = form.Role!.Value, RocketCrewId = form.RocketCrewId!.Value
            });
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();
            var member = await context.CrewMembers.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();
            var form = new CrewMemberForm {
                Id = member.Id, FirstName = member.FirstName, LastName = member.LastName,
                Role = member.Role, RocketCrewId = member.RocketCrewId
            };
            await PopulateCrews(form);
            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,Role,RocketCrewId")] CrewMemberForm form) {
            var member = await context.CrewMembers.FindAsync(id);
            if (member == null) return NotFound();
            await ValidateCrew(form);
            if (!ModelState.IsValid) {
                form.Id = id;
                await PopulateCrews(form);
                return View(form);
            }

            member.FirstName = form.FirstName;
            member.LastName = form.LastName;
            member.Role = form.Role!.Value;
            member.RocketCrewId = form.RocketCrewId!.Value;
            try {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!await context.CrewMembers.AnyAsync(m => m.Id == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return NotFound();
            var member = await context.CrewMembers.AsNoTracking().Include(m => m.RocketCrew)
                .ThenInclude(c => c!.Rocket).FirstOrDefaultAsync(m => m.Id == id);
            return member == null ? NotFound() : View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var member = await context.CrewMembers.FindAsync(id);
            if (member == null) return NotFound();
            context.CrewMembers.Remove(member);
            try {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!await context.CrewMembers.AnyAsync(m => m.Id == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task ValidateCrew(CrewMemberForm form) {
            if (form.RocketCrewId.HasValue && !await context.RocketCrews.AnyAsync(c => c.Id == form.RocketCrewId.Value)) {
                ModelState.AddModelError(nameof(form.RocketCrewId), "Select an existing crew.");
            }
        }

        private async Task PopulateCrews(CrewMemberForm form) {
            form.Crews = await context.RocketCrews.AsNoTracking().OrderBy(c => c.CrewName)
                .Select(c => new SelectListItem {
                    Value = c.Id.ToString(), Text = c.CrewName + " (" + c.Rocket!.RocketName + ")"
                }).ToListAsync();
        }
    }
}
