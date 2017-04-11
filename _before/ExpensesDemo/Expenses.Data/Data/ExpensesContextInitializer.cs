using System.Data.Entity;

namespace Expenses.Data
{
    public class ExpensesContextInitializer : DropCreateDatabaseIfModelChanges<ExpensesContext>
    {
        protected override void Seed(ExpensesContext context)
        {
            base.Seed(context);

            ExpensesDemoData.CreateNewDemoEmployee(ExpensesDemoData.DefaultEmployeeAlias);
        }
    }
}
