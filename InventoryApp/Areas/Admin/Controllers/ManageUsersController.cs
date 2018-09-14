using InventoryApp.Areas.Admin.Models;
using InventoryApp.Helpers;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Controllers
{
    public class ManageUsersController : Controller
    {
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
            }

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
                var docAdded = objUserDocuments .Count > 0 ? objUserDocuments.Where(x => x.DocumentId == doc.id).FirstOrDefault() : null;

                UserDocumentTypeModel objUserDocTypeViewModel = new UserDocumentTypeModel
                {
                    Id = doc.id,
                    Name = doc.DocumentType,
                    IsAdded = objUserDocuments != null ? true : false
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
                        AspNetUsers objUser = new AspNetUsers();
                        objUser.Name = loUserViewModel.Name;
                        objUser.Email = loUserViewModel.Email;
                        objUser.PhoneNumber = loUserViewModel.PhoneNumber;
                        objUser.Address = loUserViewModel.Address;
                        objUser.DepositAmount = loUserViewModel.DepositAmount;
                        objUser.PaymentDate = loUserViewModel.PaymentDate;
                        objUser.MembershipDuration = loUserViewModel.MembershipDuration;
                        objUser.IsPaid = loUserViewModel.IsPaid;
                        await Repository<AspNetUsers>.InsertEntity(objUser, entity => { return entity.Id; });

                        string[] objDocumentSeleced = loUserViewModel.DocumentTypes.Split(',');
                        foreach(string docId in objDocumentSeleced)
                        {
                            var objUserDocuments = Repository<AspNetUserDocumentTypes>.GetEntityListForQuery(x => x.UserId == loUserViewModel.Id && x.DocumentId == Convert.ToInt32(docId)).Item1.FirstOrDefault();
                            if(objUserDocuments == null)
                            {
                                AspNetUserDocumentTypes objDocType = new AspNetUserDocumentTypes();
                                objDocType.DocumentId = Convert.ToInt32(docId);
                                objDocType.UserId = objUser.Id;
                                await Repository<AspNetUserDocumentTypes>.InsertEntity(objDocType, entity => { return entity.id; });
                            }                            
                        }

                        TempData["SuccessMsg"] = "User has been added successfully";
                    }
                    else
                    {
                        AspNetUsers objUser = Repository<AspNetUsers>.GetEntityListForQuery(x => x.Id == loUserViewModel.Id).Item1.FirstOrDefault();
                        
                        objUser.Name = loUserViewModel.Name;
                        objUser.Email = loUserViewModel.Email;
                        objUser.PhoneNumber = loUserViewModel.PhoneNumber;
                        objUser.Address = loUserViewModel.Address;
                        objUser.DepositAmount = loUserViewModel.DepositAmount;
                        objUser.PaymentDate = loUserViewModel.PaymentDate;
                        objUser.MembershipDuration = loUserViewModel.MembershipDuration;
                        objUser.IsPaid = loUserViewModel.IsPaid;
                        await Repository<AspNetUsers>.UpdateEntity(objUser, (entity) => { return entity.Id; });
                        
                        string[] objDocumentSeleced = loUserViewModel.DocumentTypes.Split(',');
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
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "Something wrong!! Please try after sometime";
                }
            }
            return RedirectToAction("Index", "Category");
        }
    }
}