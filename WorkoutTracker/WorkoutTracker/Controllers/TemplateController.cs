using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkoutTracker.Controllers
{
    [Authorize]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> Template()
        {
            var templates = await _templateService.GetByUserAsync(CurrentUserId);
            return View(templates);
        }
        [HttpGet]
        public async Task<IActionResult> EditTemplate(int? id)
        {
            if (id == null || id == 0)
                return View(new TemplateHeader());

            var header = await _templateService.GetByIdAsync(id.Value);
            if (header == null)
                return NotFound();

            return View(header);
        }
        [HttpPost]
        public async Task<IActionResult> EditTemplate(TemplateHeader model)
        {
            if (model.Id == 0)
            {
                await _templateService.CreateAsync(CurrentUserId, model.Name, model.Description);
            }
            else
            {
                var header = await _templateService.GetByIdAsync(model.Id);
                if (header == null) return NotFound();
                if (header.UserId != CurrentUserId) return Forbid();

                await _templateService.UpdateAsync(model.Id, model.Name, model.Description);
            }

            return RedirectToAction(nameof(Template));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTemplate(int templateId)
        {
            var header = await _templateService.GetByIdAsync(templateId);
            if (header == null)
                return NotFound();

            await _templateService.DeleteAsync(templateId);
            return RedirectToAction(nameof(Template));
        }
        [HttpGet]
        public async Task<IActionResult> AddExerciseToTemplate(int templateId)
        {
            var header = await _templateService.GetByIdAsync(templateId);
            if (header == null) return NotFound();
            if (header.UserId != CurrentUserId) return Forbid();

            var available = await _templateService.GetAvailableExercisesAsync(templateId, CurrentUserId);

            ViewBag.TemplateId = templateId;
            ViewBag.TemplateName = header.Name;

            return View(available);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExerciseToTemplate(int templateId, int exerciseId)
        {
            var header = await _templateService.GetByIdAsync(templateId);
            if (header == null) return NotFound();
            if (header.UserId != CurrentUserId) return Forbid();

            await _templateService.AddExerciseAsync(templateId, exerciseId, CurrentUserId);

            return RedirectToAction(nameof(Template));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveExerciseFromTemplate(int templateId, int exerciseId)
        {
            var header = await _templateService.GetByIdAsync(templateId);
            if (header == null) return NotFound();
            if (header.UserId != CurrentUserId) return Forbid();

            await _templateService.RemoveExerciseAsync(templateId, exerciseId, CurrentUserId);

            return RedirectToAction(nameof(Template));
        }
    }
}
