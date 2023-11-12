using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;

		public UsersController(UserManager<ApplicationUser> userManager)
		{
			this.userManager = userManager;
		}
		public async Task<IActionResult> Index(string SearchValue = "")
		{
			List<ApplicationUser> users;
			if (string.IsNullOrEmpty(SearchValue))
				users = await userManager.Users.ToListAsync();
			else
				users = await userManager.Users
						.Where(user => user.Email.Trim().ToLower().Contains(SearchValue.Trim().ToLower()))
						.ToListAsync();
			return View(users);
		}
		public async Task<IActionResult> Details(string id , string ViewName = "Details")
		{
			if (id == null)
				return NotFound();
			var user = await userManager.FindByIdAsync(id);
			if (user is null)
				return NotFound();
			return View(ViewName, user);
		}
		public async Task<IActionResult> Update(string id)
		{
			return await Details(id, "Update");
		}
		[HttpPost]
		public async Task<IActionResult> Update( ApplicationUser appuser)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByIdAsync(appuser.Id);
				user.UserName = appuser.UserName;
				user.NormalizedUserName = appuser.UserName.ToUpper();
				var result = await userManager.UpdateAsync(user);
				if (result.Succeeded)
					return RedirectToAction("Index");
				foreach (var error in result.Errors)
					ModelState.AddModelError("", error.Description);
			}
			return View(appuser);
		}
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
				return NotFound();
			var user = await userManager.FindByIdAsync(id);
			if (user is null)
				return NotFound();
			var result = await userManager.DeleteAsync(user);
			if(result.Succeeded)
				return RedirectToAction("Index");
			foreach (var error in result.Errors)
				ModelState.AddModelError("", error.Description);
			return RedirectToAction("Index");
		}
	}
}
