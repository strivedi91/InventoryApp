using InventoryApp.Areas.Admin.Models;
using InventoryApp.Controllers;
using InventoryApp.Helpers;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
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
    [Authorize(Roles = "Admin")]
    public class PageAccessController : BaseController
    {
        #region public Methods
        public ActionResult Index()
        {
            PageAccessModel objPageAccess = new PageAccessModel();

            var objUsers = Repository<AspNetUsers>.GetEntityListForQuery(x => x.IsDeleted == false && x.IsAdmin == true).Item1;

            objPageAccess.objUserList.Add(new SelectListItem { Text = "-- Select --", Value = "", Selected = true });

            objPageAccess.objUserList.AddRange(objUsers
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                }));

            return View(objPageAccess);
        }

        public ActionResult GetPageAccessDetailsByUserId(string UserId)
        {
            List<Expression<Func<PageAccess, Object>>> includes = new List<Expression<Func<PageAccess, object>>>();
            Expression<Func<PageAccess, object>> IncludePages = (pages) => pages.Pages;
            includes.Add(IncludePages);

            var objPageAccess = Repository<PageAccess>.GetEntityListForQuery(x => x.UserId == UserId, null, includes).Item1;

            List<PageAccessModel> objPageAccessModel = new List<PageAccessModel>();

            objPageAccessModel.AddRange(objPageAccess
                .Select(x => new PageAccessModel
                {
                    Id = x.Id,
                    PageId = x.PageId,
                    PageName = x.Pages.PageName,
                    UserId = x.UserId,
                    IsAccessGranted = x.IsAccessGranted
                }));

            return View("~/Areas/Admin/Views/PageAccess/_PageAccess.cshtml", objPageAccessModel);
        }

        [HttpPost]
        public async Task<JsonResult> AssignPageAccess(PageAccessList foRequest)
        {
            bool IsSuccess = false;
            try
            {
                foreach (var pageAccess in foRequest.objPageAccess)
                {
                    if (pageAccess.Id == 0)
                    {
                        PageAccess objPageAccess = new PageAccess
                        {
                            PageId = pageAccess.PageId,
                            UserId = pageAccess.UserId,
                            IsAccessGranted = pageAccess.IsAccessGranted
                        };

                        await Repository<PageAccess>.InsertEntity(objPageAccess, entity => { return entity.Id; });
                    }
                    else
                    {
                        PageAccess objPageAccess = Repository<PageAccess>.GetEntityListForQuery(x => x.Id == pageAccess.Id).Item1.FirstOrDefault();

                        objPageAccess.IsAccessGranted = pageAccess.IsAccessGranted;

                        await Repository<PageAccess>.UpdateEntity(objPageAccess, (entity) => { return entity.Id; });
                    }

                    TempData["SuccessMsg"] = "Page Access has been updated.";
                }
                IsSuccess = true;
            }
            catch
            {
                IsSuccess = false;
            }

            return Json(IsSuccess);
        }
        #endregion

        #region Private Methodes
        #endregion
    }
}