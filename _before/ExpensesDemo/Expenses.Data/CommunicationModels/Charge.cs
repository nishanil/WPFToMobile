using System;
using System.Runtime.Serialization;

namespace Expenses.Data.CommunicationModels
{
    [DataContract]
    public class Charge
    {
        public Charge()
        { }

        public Charge(DbCharge dbCharge)
        {
            this.BilledAmount = dbCharge.BilledAmount;
            this.ChargeId = dbCharge.ChargeId;
            this.Description = dbCharge.Description;
            this.EmployeeId = dbCharge.EmployeeId;
            this.ExpenseDate = dbCharge.ExpenseDate;
            this.ExpenseReportId = dbCharge.ExpenseReportId;
            this.Location = dbCharge.Location;
            this.Merchant = dbCharge.Merchant;
            this.Notes = dbCharge.Notes;
            this.TransactionAmount = dbCharge.TransactionAmount;
        }

        [DataMember]
        public int ChargeId { get; set; }

        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public int? ExpenseReportId { get; set; }

        [DataMember]
        public DateTime ExpenseDate { get; set; }

        [DataMember]
        public string Merchant { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public decimal BilledAmount { get; set; }

        [DataMember]
        public decimal TransactionAmount { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Notes { get; set; }
    }
}
