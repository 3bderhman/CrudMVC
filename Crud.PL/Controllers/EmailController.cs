using Crud.BL.Helper;
using Crud.BL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crud.PL.Controllers
{
    [Authorize(Roles = "SuberAdmin, Admin, HR")]
    public class EmailController : Controller
    {
        public IActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendEmail(MailVM mail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["Mailsucceed"] = EMail.mail(mail.Email, mail.Subject, mail.Body);
                    return RedirectToAction("SendEmail");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["MailFaild"] = "Faild";
                return View(mail);
            }
        }
    }
}
