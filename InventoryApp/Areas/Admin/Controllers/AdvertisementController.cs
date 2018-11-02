using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Controllers
{
    public class AdvertisementController : Controller
    {
        string AdvertisementImagePath = ConfigurationManager.AppSettings["AdvertisementImagePath"].ToString();

        // GET: Admin/Advertisement
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload()
        {
            string outPutMessage = string.Empty;
            bool isSuccess = false;
            List<Tuple<string, string>> fileNameWithPath = new List<Tuple<string, string>>();
            try
            {
                string imageFolderName = string.Empty;

                if (Request != null
                    && Request.Files != null
                    && Request.Files.Count > 0)
                {
                    imageFolderName = Request.Files.Keys[0];
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        if (file != null)
                        {

                            string fileName = file.FileName;
                            string path = Path.Combine(Server.MapPath(AdvertisementImagePath), imageFolderName);

                            if (!Directory.Exists(path))
                            { Directory.CreateDirectory(path); }

                            file.SaveAs(path + "\\" + fileName);

                            fileNameWithPath.Add(new Tuple<string, string>(path + "\\" + fileName, fileName));

                        }
                    }
                    isSuccess = true;
                    outPutMessage = "File Uploaded Successfully!";
                }
                else
                { outPutMessage = "No files selected."; }
            }
            catch (Exception ex)
            { outPutMessage = ex.Message; }

            return Json(new { isSuccess, outPutMessage, fileNameWithPath }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getUploadedFile()
        {
            string path = Server.MapPath(AdvertisementImagePath);
            List<SelectListItem> fileNameWithPath = new List<SelectListItem>();
            if (Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);
                FileInfo[] files = info.GetFiles("*.*");

                foreach (FileInfo file in files)
                {
                    SelectListItem item = new SelectListItem();
                    string fileName = file.Name;
                    item.Value = fileName;
                    item.Text = path + "\\" + fileName;
                    fileNameWithPath.Add(item);
                }
            }

            return Json(new { fileNameWithPath }, JsonRequestBehavior.AllowGet);
        }

        public bool DeleteUploadedFile(string Id, string filename)
        {
            string path = Path.Combine(Server.MapPath(AdvertisementImagePath), Id, filename);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return true;
        }

    }
}