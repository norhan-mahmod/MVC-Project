using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RolesController(
            RoleManager<ApplicationRole> roleManager ,
            UserManager<ApplicationUser> userManager
            )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();

            return View(roles);
        }
        public IActionResult Create()
        {
            return View(new ApplicationRole());
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(role);
        }
        public async Task<IActionResult> Details(string id , string ViewName = "Details")
        {
            if (id == null)
                return NotFound();
            var role = await roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();
            return View(role);
        }
        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }
        [HttpPost]
        public async Task<IActionResult> Update(ApplicationRole appRole)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(appRole.Id);
                if (role is null)
                    return NotFound();
                role.Name = appRole.Name;
                role.NormalizedName = appRole.Name.ToUpper();
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(appRole);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();
            var role = await roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            if (roleId == null)
                return NotFound();
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            List<UserInRoleViewModel> usersInRole = new List<UserInRoleViewModel>();
            foreach(var user in await userManager.Users.ToListAsync())
            {
                var userInRoleViewModel = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                    userInRoleViewModel.IsSelected = true;
                else
                    userInRoleViewModel.IsSelected = false;
                usersInRole.Add(userInRoleViewModel);
            }
            ViewBag.RoleId = roleId;
            return View(usersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId , List<UserInRoleViewModel> users)
        {
            if (roleId == null)
                return NotFound();
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                foreach(var user in users)
                {
                    var appUser = await userManager.FindByIdAsync(user.UserId);
                    if (user.IsSelected && !(await userManager.IsInRoleAsync(appUser, role.Name)))
                        await userManager.AddToRoleAsync(appUser, role.Name);
                    if (!user.IsSelected && (await userManager.IsInRoleAsync(appUser, role.Name)))
                        await userManager.RemoveFromRoleAsync(appUser, role.Name);
                }
                return RedirectToAction("Update", new { id = role.Id });
            }
            return View(users);
        }
    }

}
