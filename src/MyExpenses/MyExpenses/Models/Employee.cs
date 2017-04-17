using System;
using System.Collections.Generic;
using System.Text;

namespace MyExpenses.Models
{
    public class Employee : BaseDataObject
    {

        public string Name { get; set; }

        public string Alias { get; set; }

        public string Manager { get; set; }
    }
}
