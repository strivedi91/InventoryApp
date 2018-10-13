using InventoryApp.Helpers;
using InventoryApp_DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Models
{
    public class OffersModel
    {
        public OffersModel()
        {
            loOfferList = new List<OffersViewModel>();
        }

        public Int64 inRecordCount { get; set; }
        public int inPageIndex { get; set; }
        public Pager Pager { get; set; }

        public List<OffersViewModel> loOfferList { get; set; }
    }

    public class OffersViewModel
    {
        public OffersViewModel()
        {
            objProductList = new List<SelectListItem>();
            objCategoryList = new List<SelectListItem>();
        }

        public int id { get; set; }

        [Required(ErrorMessage = "Offer Code should not be empty.")]
        public string OfferCode { get; set; }

        public string OfferDescription { get; set; } 

        public decimal? FlatDiscount { get; set; }

        public int? PercentageDiscount { get; set; }

        public int? ProductId { get; set; }

        public string ProductName { get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Start Date should not be empty.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End Date should not be empty.")]
        public DateTime? EndDate { get; set; }

        public string stSearch { get; set; }

        public int inPageIndex { get; set; }

        public int inPageSize { get; set; }

        public string stSortColumn { get; set; }

        public Int64 inRowNumber { get; set; }

        public List<SelectListItem> objProductList { get; set; }

        public List<SelectListItem> objCategoryList { get; set; }
    }
}