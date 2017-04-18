using System;
using System.Collections.Generic;
using System.Text;

namespace MyExpenses.Models
{
    public enum Category
    {
        Meal,
        Taxi,
        Flight,
        Hotel,
        Other
    }

    public class Charge : BaseDataObject
    {

        //public int ChargeId { get; set; }

  
        public Category ChargeCategory { get; set; }


        public string EmployeeId { get; set; }


        public string ExpenseReportId { get; set; }


        public DateTime ExpenseDate { get; set; }


        public string Merchant { get; set; }


        public string Location { get; set; }


        public decimal BilledAmount { get; set; }


        public decimal TransactionAmount { get; set; }


        public string Description { get; set; }


        public string Notes { get; set; }
    }
}
