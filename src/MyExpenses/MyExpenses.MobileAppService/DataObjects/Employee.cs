using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyExpenses.MobileAppService.DataObjects
{
    public class Employee : EntityData
    {

        public string Name { get; set; }

        public string Alias { get; set; }

        public string Manager { get; set; }
    }
}