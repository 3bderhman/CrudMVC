using Crud.BL.Helper;
using Crud.BL.Model;
using Crud.DAL.External;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Crud.PL.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> manger;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> Manger, SignInManager<ApplicationUser> signInManager)
        {
            manger = Manger;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        IsAgree = model.IsAgree,
                    };
                    var result = await manger.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
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
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var User = await manger.FindByNameAsync(model.UserName);
                    if(User != null)
                    {
                        var result = await signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Wrong Password");
                        }
                    }
                }
                ModelState.AddModelError("", "Account Invalid");
                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SingOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            var user = await manger.FindByEmailAsync(model.Email);
            if(user != null)
            {
                var Token = await manger.GeneratePasswordResetTokenAsync(user);
                var PasswordResetLink = Url.Action("ResetPassword", "Account", new { Email = model.Email, Token = Token }, Request.Scheme);
                EMail.mail(user.Email, "PasswordReset", PasswordResetLink);
                return RedirectToAction("ConfirmForgetPassword");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ConfirmForgetPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string Email, string token)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await manger.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await manger.ResetPasswordAsync(user, model.Token, model.Password);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("ConfirmResetPassword");
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
                    return View(model);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ConfirmResetPassword()
        {
            return View();
        }
    }
}
