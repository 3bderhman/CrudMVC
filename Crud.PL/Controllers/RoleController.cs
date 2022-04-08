using Crud.BL.Model;
using Crud.DAL.External;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Crud.PL.Controllers
{
    [Authorize (Roles = "SuberAdmin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var Rule = roleManager.Roles;
            return View(Rule);
        }
        public async Task<IActionResult> Details(string id)
        {
            var Rule = await roleManager.FindByIdAsync(id);
            return View(Rule);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleVM model)
        {
            try
            {
                var role = new IdentityRole()
                {
                    Name = model.Name,
                };
                var result = await roleManager.CreateAsync(role);
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
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var Role = await roleManager.FindByIdAsync(id);
            return View(Role);
        }
        [HttpPost]
        public async Task<IActionResult> Update(IdentityRole model)
        {
            try
            {

                var Role = await roleManager.FindByIdAsync(model.Id);
                if (Role != null)
                {
                    Role.Name = model.Name;
                    Role.NormalizedName = model.Name.ToUpper();
                    var result = await roleManager.UpdateAsync(Role);
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
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Role");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var Role = await roleManager.FindByIdAsync(id);
            return View(Role);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(IdentityRole model)
        {
            try
            {

                var Role = await roleManager.FindByIdAsync(model.Id);
                if (Role != null)
                {
                    var result = await roleManager.DeleteAsync(Role);
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
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Role");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddOrDeleteUserInRole(string id)
        {
            ViewBag.RoleId = id;
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var model = new List<UserInRoleVM>();
                foreach (var user in userManager.Users)
                {
                    var userInRole = new UserInRoleVM()
                    {
                        Id = user.Id,
                        UserName = user.UserName
                    };
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        userInRole.IsSelected = true;
                    }
                    else
                    {
                        userInRole.IsSelected = false;
                    }
                    model.Add(userInRole);
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Update");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddOrDeleteUserInRole(List<UserInRoleVM> model, string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            for(int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].Id);
                IdentityResult result = null;
                if(model[i].IsSelected && !(await userManager.IsInRoleAsync(user , role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result= await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
            }
            return RedirectToAction("Update", new {id = id});
        }
    }
}
