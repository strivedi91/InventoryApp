using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryApp.Models.ApiModels
{
    public class SuggestionModel
    {
        public int Id { get; set; }        
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string Suggestion { get; set; }
    }
}