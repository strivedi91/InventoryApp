using InventoryApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryApp.Areas.Admin.Models
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            loCategoryList = new List<CategoryViewModel>();
        }
        public Int64 inRecordCount { get; set; }
        public List<CategoryViewModel> loCategoryList { get; set; }
        public int inPageIndex { get; set; }
        public Pager Pager { get; set; }
    }
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name should not be empty.")]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; } 

        public int ParentId { get; set; }

        [Range(1, 100, ErrorMessage = "GST should be 1 to 100.")]
        public decimal? GST { get; set; }

        public string stSearch { get; set; }

        public int inPageIndex { get; set; }

        public int inPageSize { get; set; }

        public string stSortColumn { get; set; }

        public Int64 inRowNumber { get; set; }
    }
}