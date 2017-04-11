using Expenses.Data.CommunicationModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Expenses.Data
{
    [DataContract(Name="Charge", IsReference = true)]
    public class DbCharge
    {
        public DbCharge()
        { }

        public DbCharge(Charge charge)
        {
            this.BilledAmount = charge.BilledAmount;
            this.ChargeId = charge.ChargeId;
            this.Description = charge.Description;
            this.EmployeeId = charge.EmployeeId;
            this.ExpenseDate = charge.ExpenseDate;
            this.ExpenseReportId = charge.ExpenseReportId;
            this.Location = charge.Location;
            this.Merchant = charge.Merchant;
            this.Notes = charge.Notes;
            this.TransactionAmount = charge.TransactionAmount;
        }

        [DataMember]
        [Key]
        public int ChargeId { get; set; }
        [DataMember]
        public int EmployeeId { get; set; }
        public virtual DbEmployee Employee { get; set; }
        [DataMember]
        public int? ExpenseReportId { get; set; }
        public virtual DbExpenseReport ExpenseReport { get; set; }
        [DataMember]
        [Column(TypeName = "date")]
        public DateTime ExpenseDate { get; set; }
        [DataMember]
        [Required]
        [StringLength(50)]
        public string Merchant { get; set; }
        [DataMember]
        [Required]
        [StringLength(50)]
        public string Location { get; set; }
        [DataMember]
        [Column(TypeName = "smallmoney")]
        public decimal BilledAmount { get; set; }
        [DataMember]
        [Column(TypeName = "smallmoney")]
        public decimal TransactionAmount { get; set; }
        [DataMember]
        [Required]
        [StringLength(250)]
        public string Description { get; set; }
        [DataMember]
        [StringLength(250)]
        public string Notes { get; set; }
    }
}
