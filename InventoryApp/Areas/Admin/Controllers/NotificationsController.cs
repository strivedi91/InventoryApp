using FCM.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Controllers
{
    public class NotificationsController : Controller
    {
        // GET: Admin/Notifications
        public ActionResult Index()
        {
            return View();
        }

        public async Task SendNotificaitons(string deviceid)
        {            
            //You can get the server Key by accessing the url/ Você pode obter a chave do servidor acessando a url 
            //https://console.firebase.google.com/project/MY_PROJECT/settings/cloudmessaging";
            using (var sender = new Sender("AAAAF9xxwm8:APA91bHZWHkQBMFJLbo5tCtA48IoMDPGfNzPBEgs768oq9I72W_BvQxwugCdloufW3lTa9t0mMXgl3teDLC0N-9gQ8p1_twLbToRwFxKOLM2yop2joX6iXIsbmsS5ew9-wXeNT3HqJGE"))
            {
                var message = new Message
                {
                    RegistrationIds = new List<string> { deviceid },
                    Notification = new Notification
                    {
                        Title = "Test from FCM.Net",
                        Body = $"Hello World@!{DateTime.Now.ToString()}"
                    }
                };
                var result = await sender.SendAsync(message);
                Console.WriteLine($"Success: {result.MessageResponse.Success}");

                var json = "{\"notification\":{\"title\":\"json message\",\"body\":\"works like a charm!\"},\"to\":\"" + registrationId + "\"}";
                result = await sender.SendAsync(json);
                Console.WriteLine($"Success: {result.MessageResponse.Success}");
            }
        }
    }
}