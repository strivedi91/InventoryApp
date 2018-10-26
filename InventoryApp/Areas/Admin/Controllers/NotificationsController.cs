using FCM.Net;
using InventoryApp.Areas.Admin.Models;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace InventoryApp.Areas.Admin.Controllers
{
    public class NotificationsController : Controller
    {
        // GET: Admin/Notifications
        public ActionResult Index()
        {
            //SendNotificaitons("cgSYtLu6oLc:APA91bEXKljdcKx517qZGeg2xCR8EaE5vEnPf6MSG5c9X8rcvTW7uk1nPoRJV5TzllhqqguK8KdAJa1Ir3dmAALGIzZ_NTS-VXbCVT7G7eBeW0PFw_Q3SgngIIpPzt5BCA0BSB1EnQby");
            return View();
        }

        [HttpGet]
        public ActionResult SendNotification()
        {
            NotificaitonViewModel notificaitonViewModel = new NotificaitonViewModel();
            return View("~/Areas/Admin/Views/Notifications/SendNotification.cshtml", notificaitonViewModel);
        }

        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> SendNotification(NotificaitonViewModel notificaitonViewModel)
        {
            if (notificaitonViewModel != null)
            {
                try
                {
                    NotificationHistory objNotification = new NotificationHistory()
                    {
                        Title = notificaitonViewModel.Title,
                        NotificationText = notificaitonViewModel.NotificaitonText,
                        CreatedBy = User.Identity.GetUserId()
                    };

                    await Repository<NotificationHistory>.InsertEntity(objNotification, entity => { return entity.Id; });
                    await SendNotificaitons(notificaitonViewModel.Title, notificaitonViewModel.NotificaitonText);
                    TempData["SuccessMsg"] = "Notification Sent Successfully";
                }

                catch (Exception ex)
                {
                    TempData["ErrorMsg"] = "Something wrong!! Please try after sometime";
                }
            }
            return RedirectToAction("Index", "Notifications");
        }

        public async Task SendNotificaitons(string Title, string notificationText)
        {
            //You can get the server Key by accessing the url/ Você pode obter a chave do servidor acessando a url 
            //https://console.firebase.google.com/project/MY_PROJECT/settings/cloudmessaging";
            var users = Repository<AspNetUsers>.GetEntityListForQuery(x => x.IsDeleted == false).Item1.Select(x => x.DeviceId).ToList();
            using (var sender = new Sender("AAAAF9xxwm8:APA91bHZWHkQBMFJLbo5tCtA48IoMDPGfNzPBEgs768oq9I72W_BvQxwugCdloufW3lTa9t0mMXgl3teDLC0N-9gQ8p1_twLbToRwFxKOLM2yop2joX6iXIsbmsS5ew9-wXeNT3HqJGE"))
            {
                var message = new Message
                {
                    RegistrationIds = users,
                    Notification = new Notification
                    {
                        Title = Title,
                        Body = notificationText
                    }
                };
                var result = await sender.SendAsync(message);
            }
        }
    }
}