using Crud.DAL.External;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Crud.PL.Controllers
{
    [Authorize(Roles = "SuberAdmin, Admin, HR")]

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> manager;

        public UserController(UserManager<ApplicationUser> manager)
        {
            this.manager = manager;
        }
        public IActionResult Index()
        {
            var user = manager.Users;
            return View(user);
        }
        public async Task<IActionResult> Details(string id)
        {
            var user = await manager.FindByIdAsync(id);
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await manager.FindByIdAsync(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ApplicationUser Model)
        {
            var user = await manager.FindByIdAsync(Model.Id);
            if(user != null)
            {
                user.UserName = Model.UserName;
                user.NormalizedUserName = Model.UserName.ToUpper();
                user.Email = Model.Email;
                user.NormalizedEmail = Model.Email.ToUpper();
                user.SecurityStamp = Model.SecurityStamp;

                var result = await manager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(Model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Account");
                return View(Model);
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await manager.FindByIdAsync(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser Model)
        {
            var user = await manager.FindByIdAsync(Model.Id);
            if(user != null)
            {
                var result = await manager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(Model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Account");
                return View(Model);
            }
        }
    }
}
