using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace InventoryApp.Helpers
{
    public class EmailHelper
    {
        public static bool sendEmail(string fsToEmail, string fsFromEmail, string fsSubject, string fsBody)
        {
            try
            {                
                using (MailMessage mm = new MailMessage(fsFromEmail, fsToEmail))
                {
                    mm.Subject = fsSubject;
                    mm.Body = fsBody;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;

                    NetworkCredential NetworkCred = new NetworkCredential(ConfigurationManager.AppSettings["SystemEmail"], ConfigurationManager.AppSettings["EmailPassword"]);
                    smtp.Credentials = NetworkCred;

                    smtp.Port = 587;
                    //smtp.Port = 25;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.UseDefaultCredentials = false;
                    //smtp.Host = "smtp.gmail.com";
                    smtp.Send(mm);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}