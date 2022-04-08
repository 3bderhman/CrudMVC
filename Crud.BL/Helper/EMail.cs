using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Crud.BL.Model;

namespace Crud.BL.Helper
{
    public static class EMail
    {
        public static string mail(string Email, string Subject, string Body)
        {
            try
            {
                string MyGmail = "abderhmansaid58@gmail.com";
                var smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(MyGmail, "22255577");
                smtp.Send(MyGmail, Email, Subject, Body);
                return "succeed";
            }
            catch (Exception ex)
            {
                return "Faild";
            }
        }
    }
}
