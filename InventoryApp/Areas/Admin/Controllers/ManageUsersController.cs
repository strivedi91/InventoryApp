using InventoryApp.Areas.Admin.Models;
using InventoryApp.Helpers;
using InventoryApp.Models;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Controllers
{
    public class ManageUsersController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        Random radomObj = new Random();
        public ManageUsersController()
        {
        }

        public ManageUsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin/ManageUsers
        public ActionResult Index()
        {
            UserViewModel foRequest = new UserViewModel();
            foRequest.stSortColumn = "ID ASC";
            return View("~/Areas/Admin/Views/ManageUsers/ManageUser.cshtml", getUserList(foRequest));
        }

        public ActionResult searchUser(UserViewModel foSearchRequest)
        {
            UserModel loUserModel = getUserList(foSearchRequest);
            return PartialView("~/Areas/Admin/Views/ManageUsers/_ManageUser.cshtml", loUserModel);
        }

        public UserModel getUserList(UserViewModel foRequest)
        {
            if (foRequest.inPageSize <= 0)
                foRequest.inPageSize = 10;

            if (foRequest.inPageIndex <= 0)
                foRequest.inPageIndex = 1;

            if (foRequest.stSortColumn == "")
                foRequest.stSortColumn = null;

            if (foRequest.stSearch == "")
                foRequest.stSearch = null;


            Func<IQueryable<AspNetUsers>, IOrderedQueryable<AspNetUsers>> orderingFunc =
            query => query.OrderBy(x => x.Id);

            Expression<Func<AspNetUsers, bool>> expression = null;

            if (!string.IsNullOrEmpty(foRequest.stSearch))
            {
                foRequest.stSearch = foRequest.stSearch.Replace("%20", " ");
                expression = x => x.Name.ToLower().Contains(foRequest.stSearch.ToLower()) && x.IsDeleted == false;
            }
            else
                expression = x => x.IsDeleted == false;

            if (!string.IsNullOrEmpty(foRequest.stSortColumn))
            {
                switch (foRequest.stSortColumn)
                {
                    case "ID DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Id);
                        break;
                    case "ID ASC":
                        orderingFunc = q => q.OrderBy(s => s.Id);
                        break;
                    case "Name DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Name);
                        break;
                    case "Name ASC":
                        orderingFunc = q => q.OrderBy(s => s.Name);
                        break;
                    case "Email DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Email);
                        break;
                    case "Email ASC":
                        orderingFunc = q => q.OrderBy(s => s.Email);
                        break;
                }
            }

            (List<AspNetUsers>, int) objUsers = Repository<AspNetUsers>.GetEntityListForQuery(expression, orderingFunc, null, foRequest.inPageIndex, foRequest.inPageSize);


            UserModel objUserViewModel = new UserModel();

            objUserViewModel.inRecordCount = objUsers.Item2;
            objUserViewModel.inPageIndex = foRequest.inPageIndex;
            objUserViewModel.Pager = new Pager(objUsers.Item2, foRequest.inPageIndex);

            if (objUsers.Item1.Count > 0)
            {
                foreach (var user in objUsers.Item1)
                {
                    objUserViewModel.loUserList.Add(new UserViewModel
                    {
                        Id = user.Id,
                        Address = user.Address,
                        DepositAmount = user.DepositAmount,
                        Email = user.Email,
                        IsPaid = user.IsPaid,
                        Name = user.Name,
                        MembershipDuration = user.MembershipDuration,
                        PaymentDate = user.PaymentDate,
                        PhoneNumber = user.PhoneNumber
                    });
                }
            }

            return objUserViewModel;
        }

        [HttpGet]
        public ActionResult AddEditUser(string id = "")
        {
            UserViewModel objUserViewModel = new UserViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                AspNetUsers objUser = Repository<AspNetUsers>.GetEntityListForQuery(x => x.Id == id).Item1.FirstOrDefault();

                objUserViewModel = new UserViewModel
                {
                    Id = objUser.Id,
                    Name = objUser.Name,
                    Email = objUser.Email,
                    PhoneNumber = objUser.PhoneNumber,
                    Address = objUser.Address,
                    DepositAmount = objUser.DepositAmount,
                    PaymentDate = objUser.PaymentDate,
                    IsPaid = objUser.IsPaid,
                    MembershipDuration = objUser.MembershipDuration,
                    loDocumentTypeList = getDocumentType(id)
                };
            }
            else
            {
                objUserViewModel = new UserViewModel
                {
                    loDocumentTypeList = getDocumentType(id)
                };
            }
            return View(objUserViewModel);
        }

        private List<UserDocumentTypeModel> getDocumentType(string id)
        {
            List<UserDocumentTypeModel> objUserDocumentTypeList = new List<UserDocumentTypeModel>();
            var objDocumentList = Repository<DocumentTypes>.GetEntityListForQuery(null).Item1;

            var objUserDocuments = Repository<AspNetUserDocumentTypes>.GetEntityListForQuery(x => x.UserId == id).Item1;
            foreach (var doc in objDocumentList)
            {
                var docAdded = objUserDocuments.Count > 0 ? objUserDocuments.Where(x => x.DocumentId == doc.id).FirstOrDefault() : null;

                UserDocumentTypeModel objUserDocTypeViewModel = new UserDocumentTypeModel
                {
                    Id = doc.id,
                    Name = doc.DocumentType,
                    IsAdded = docAdded != null ? true : false
                };
                objUserDocumentTypeList.Add(objUserDocTypeViewModel);
            }
            return objUserDocumentTypeList;
        }

        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> AddEditUser(UserViewModel loUserViewModel)
        {
            if (loUserViewModel != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(loUserViewModel.Id))
                    {
                        var user = new ApplicationUser
                        {
                            UserName = loUserViewModel.Email,
                            Name = loUserViewModel.Name,
                            Email = loUserViewModel.Email,
                            PhoneNumber = loUserViewModel.PhoneNumber,
                            Address = loUserViewModel.Address,
                            DepositAmount = loUserViewModel.DepositAmount,
                            PaymentDate = loUserViewModel.PaymentDate,
                            MembershipDuration = loUserViewModel.MembershipDuration,
                            IsPaid = loUserViewModel.IsPaid,
                            IsActive = true
                        };
                        string password = GenerateStrongPassword(8);
                        var result = await UserManager.CreateAsync(user, password);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            sendPasswordEmail(user, password);

                            string[] objDocumentSeleced = loUserViewModel.DocumentTypes.Split(',');
                            foreach (string docId in objDocumentSeleced)
                            {
                                AspNetUserDocumentTypes objDocType = new AspNetUserDocumentTypes();
                                objDocType.DocumentId = Convert.ToInt32(docId);
                                objDocType.UserId = user.Id;
                                await Repository<AspNetUserDocumentTypes>.InsertEntity(objDocType, entity => { return entity.id; });

                            }

                            TempData["SuccessMsg"] = "User has been added successfully";
                        }
                        else
                        {
                            TempData["ErrorMsg"] = result.Errors.FirstOrDefault();
                        }

                    }
                    else
                    {
                        AspNetUsers objUser = Repository<AspNetUsers>.GetEntityListForQuery(x => x.Id == loUserViewModel.Id).Item1.FirstOrDefault();
                        objUser.Id = loUserViewModel.Id;
                        objUser.Name = loUserViewModel.Name;
                        objUser.Email = loUserViewModel.Email;
                        objUser.PhoneNumber = loUserViewModel.PhoneNumber;
                        objUser.Address = loUserViewModel.Address;
                        objUser.DepositAmount = loUserViewModel.DepositAmount;
                        objUser.PaymentDate = loUserViewModel.PaymentDate;
                        objUser.MembershipDuration = loUserViewModel.MembershipDuration;
                        objUser.IsPaid = loUserViewModel.IsPaid;
                        await Repository<AspNetUsers>.UpdateUserEntity(objUser, (entity) => { return entity.Id; });

                        string[] objDocumentSeleced = loUserViewModel.DocumentTypes.Split(',');

                        var objUserDocuments = Repository<AspNetUserDocumentTypes>.GetEntityListForQuery(x => x.UserId == objUser.Id).Item1;
                        foreach (var docExist in objUserDocuments)
                        {
                            await Repository<AspNetUserDocumentTypes>.DeleteEntity(docExist, entity => { return entity.id; });
                        }

                        foreach (string docId in objDocumentSeleced)
                        {
                            AspNetUserDocumentTypes objDocType = new AspNetUserDocumentTypes();
                            objDocType.DocumentId = Convert.ToInt32(docId);
                            objDocType.UserId = objUser.Id;
                            await Repository<AspNetUserDocumentTypes>.InsertEntity(objDocType, entity => { return entity.id; });
                        }


                        TempData["SuccessMsg"] = "User has been updated successfully";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMsg"] = "Something wrong!! Please try after sometime";
                }
            }
            return RedirectToAction("Index", "ManageUsers");
        }
        public bool checkEmail(string email, string userid)
        {
            AspNetUsers objUser = Repository<AspNetUsers>.GetEntityListForQuery(x => x.Email == email).Item1.FirstOrDefault();
            if (objUser != null && objUser.Id.ToLower() != userid.ToLower())
                return true;
            else
                return false;
        }

        public async Task<ActionResult> DeleteUser(string ID)
        {
            string liSuccess = "success";
            string lsMessage = string.Empty;

            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    AspNetUsers objUser = Repository<AspNetUsers>.GetEntityListForQuery(x => x.Id == ID).Item1.FirstOrDefault();
                    objUser.IsDeleted = true;
                    await Repository<AspNetUsers>.UpdateUserEntity(objUser, (entity) => { return entity.Id; });

                    TempData["SuccessMsg"] = "User has been deleted successfully";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMsg"] = "Something wrong!! Please try after sometime";
                }
            }

            return this.Json(new UserViewModel { Id = liSuccess });
        }


        public string GenerateStrongPassword(int length)
        {
            String generatedPassword = "";
            //create constant strings for each type of characters
            string alphaCaps = "QWERTYUIOPASDFGHJKLZXCVBNM";
            string alphaLow = "qwertyuiopasdfghjklzxcvbnm";
            string numerics = "1234567890";
            string special = "@#$";
            //create another string which is a concatenation of all above
            string allChars = alphaCaps + alphaLow + numerics + special;

            if (length < 4)
                throw new Exception("Number of characters should be greater than 7.");

            int pLower, pUpper, pNumber, pSpecial;
            string posArray = "0123456789";
            if (length < posArray.Length)
                posArray = posArray.Substring(0, length);
            pLower = getRandomPosition(ref posArray);
            pUpper = getRandomPosition(ref posArray);
            pNumber = getRandomPosition(ref posArray);
            pSpecial = getRandomPosition(ref posArray);


            for (int i = 0; i < length; i++)
            {
                if (i == pLower)
                    generatedPassword += getRandomChar(alphaCaps);
                else if (i == pUpper)
                    generatedPassword += getRandomChar(alphaLow);
                else if (i == pNumber)
                    generatedPassword += getRandomChar(numerics);
                else if (i == pSpecial)
                    generatedPassword += getRandomChar(special);
                else
                    generatedPassword += getRandomChar(allChars);
            }
            return generatedPassword;
        }

        private string getRandomChar(string fullString)
        {

            return fullString.ToCharArray()[(int)Math.Floor(radomObj.NextDouble() * fullString.Length)].ToString();
        }

        private int getRandomPosition(ref string posArray)
        {
            int pos;
            string randomChar = posArray.ToCharArray()[(int)Math.Floor(radomObj.NextDouble()
                                           * posArray.Length)].ToString();
            pos = int.Parse(randomChar);
            posArray = posArray.Replace(randomChar, "");
            return pos;
        }

        private bool sendPasswordEmail(ApplicationUser user, string password)
        {
            try
            {
                string lsFrom = "no-replay@thgodowninventorryapp.com";

                string lsToMails = user.Email;

                string lsSubject = string.Empty;

                string lsEmailBody = string.Empty;

                lsSubject = "Your new inventorry app account password";

                using (StreamReader loStreamReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Views/Shared/EmailTemplates/UserAccountCreation.html")))
                {
                    string lsLine = string.Empty;
                    while ((lsLine = loStreamReader.ReadLine()) != null)
                    {
                        lsEmailBody += lsLine;
                    }
                }

                lsEmailBody = lsEmailBody.Replace("{Name}", user.Name);
                lsEmailBody = lsEmailBody.Replace("{UserName}", user.UserName);
                lsEmailBody = lsEmailBody.Replace("{Password}", password);
                lsEmailBody = lsEmailBody.Replace("{LoginUrl}", ConfigurationManager.AppSettings["SiteAddress"] + Url.Action("Login", "Account", ""));
                lsEmailBody = lsEmailBody.Replace("{SiteUrl}", ConfigurationManager.AppSettings["SiteAddress"]);


                return EmailHelper.sendEmail(lsToMails, lsFrom, lsSubject, lsEmailBody);
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }


}