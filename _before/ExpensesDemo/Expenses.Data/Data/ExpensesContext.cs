using System.Data.Entity;

namespace Expenses.Data
{
    public class ExpensesContext : DbContext
    {
        public ExpensesContext()
            : base("name=DefaultConnection")
        {
            Database.SetInitializer<ExpensesContext>(new ExpensesContextInitializer());
        }

        protected override void Dispose(bool disposing)
        {
            this.Configuration.LazyLoadingEnabled = false;
            base.Dispose(disposing);
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<DbCharge> Charges { get; set; }
        public virtual DbSet<DbExpenseReport> Reports { get; set; }
        public virtual DbSet<DbEmployee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbCharge>()
                .Property(e => e.BilledAmount)
                .HasPrecision(10, 4);

            modelBuilder.Entity<DbCharge>()
                .Property(e => e.TransactionAmount)
                .HasPrecision(10, 4);

            modelBuilder.Entity<DbEmployee>()
                .HasMany(e => e.ExpenseReports)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DbExpenseReport>()
                .Property(e => e.Amount)
                .HasPrecision(10, 4);

            modelBuilder.Entity<DbExpenseReport>()
                .HasMany(e => e.Charges)
                .WithOptional(e => e.ExpenseReport)
                .WillCascadeOnDelete();
        }
    }
}
