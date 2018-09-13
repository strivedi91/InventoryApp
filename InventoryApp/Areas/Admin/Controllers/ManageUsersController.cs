using InventoryApp.Areas.Admin.Models;
using InventoryApp.Helpers;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    }
}