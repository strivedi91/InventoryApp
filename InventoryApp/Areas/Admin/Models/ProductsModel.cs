using InventoryApp.Helpers;
using InventoryApp_DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Models
{
    public class ProductsModel
    {
        public ProductsModel()
        {
            loProductList = new List<ProductsViewModel>();
        }

        public Int64 inRecordCount { get; set; }
        public int inPageIndex { get; set; }
        public Pager Pager { get; set; }

        public List<ProductsViewModel> loProductList { get; set; }
    }

    public class ProductsViewModel
    {
        public ProductsViewModel() {
            objCategoryList = new List<SelectListItem>();
        }

        public int id { get; set; }

        [Required(ErrorMessage = "Product name should not be empty.")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string Brand { get; set; }

        [Required(ErrorMessage = "Price should not be empty.")]
        public decimal? Price { get; set; }

        public decimal? MinimumSellingPrice { get; set; }

        public decimal? OfferPrice { get; set; }

        [Required(ErrorMessage = "Quantity name should not be empty.")]
        public int? Quantity { get; set; }

        public int? MOQ { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select Category.")]
        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string stSearch { get; set; }

        public int inPageIndex { get; set; }

        public int inPageSize { get; set; }

        public string stSortColumn { get; set; }

        public Int64 inRowNumber { get; set; }

        public List<SelectListItem> objCategoryList { get; set; }

        public List<TierPricing> objTierPricing { get; set; }

        public string lstTierPircing { get; set; }

        public HttpPostedFileBase[] loFiles { get; set; }
    }
}