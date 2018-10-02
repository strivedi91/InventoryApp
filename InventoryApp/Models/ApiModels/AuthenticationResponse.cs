using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryApp.Models.ApiModels
{
    public class AuthenticationResponse
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public bool FirstTimeLogin { get; set; }
        public string Address { get; set; }
        public int CartCount { get; set; }
    }
}