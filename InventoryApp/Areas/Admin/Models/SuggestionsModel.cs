using InventoryApp.Helpers;
using InventoryApp_DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Models
{
    public class SuggestionsModel
    {
        public SuggestionsModel()
        {
            loSuggestionList = new List<SuggestionsViewModel>();
        }

        public Int64 inRecordCount { get; set; }
        public int inPageIndex { get; set; }
        public Pager Pager { get; set; }

        public List<SelectListItem> objCategoryList { get; set; }
        public List<SuggestionsViewModel> loSuggestionList { get; set; }
    }

    public class SuggestionsViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserContectNumber { get; set; }

        public string Suggestion { get; set; }

        public string stSearch { get; set; }

        public int inPageIndex { get; set; }

        public int inPageSize { get; set; }

        public string stSortColumn { get; set; }

        public Int64 inRowNumber { get; set; }
    }
}