using System.Data.Entity;
using System.Linq;

namespace Expenses.Data
{
    public class ExpensesRepository
    {
        public DbCharge GetCharge(int chargeId)
        {
            using (var context = new ExpensesContext())
            {
                return context.Charges.Include(r => r.ExpenseReport)
                    .SingleOrDefault(i => i.ChargeId == chargeId);
            }
        }

        public DbEmployee GetEmployee(int employeeId)
        {
            using (var context = new ExpensesContext())
            {
                return context.Employees.Include(c => c.Charges).Include(r => r.ExpenseReports)
                    .SingleOrDefault(i => i.EmployeeId == employeeId);
            }
        }

        public DbEmployee GetEmployee(string employeeAlias)
        {
            using (var context = new ExpensesContext())
            {
                return context.Employees.Include(c => c.Charges).Include(r => r.ExpenseReports)
                    .SingleOrDefault(i => i.Alias == employeeAlias);
            }
        }

        public DbExpenseReport GetExpenseReport(int reportId)
        {
            using (var context = new ExpensesContext())
            {
                return context.Reports.Include(r => r.Charges)
                    .SingleOrDefault(i => i.ExpenseReportId == reportId);
            }
        }

        public DbCharge SaveCharge(DbCharge charge)
        {
            using (var context = new ExpensesContext())
            {
                if (charge.ChargeId != 0)
                {
                    context.Charges.Attach(charge);
                    context.Entry<DbCharge>(charge).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    context.Charges.Add(charge);
                }

                context.SaveChanges();
                return charge;
            }
        }

        public DbEmployee SaveEmployee(DbEmployee employee)
        {
            using (var context = new ExpensesContext())
            {
                if (employee.EmployeeId != 0)
                {
                    context.Employees.Attach(employee);
                    context.Entry<DbEmployee>(employee).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    context.Employees.Add(employee);
                }

                context.SaveChanges();
                return employee;
            }
        }

        public DbExpenseReport SaveExpenseReport(DbExpenseReport report)
        {
            using (var context = new ExpensesContext())
            {
                if (report.ExpenseReportId != 0)
                {
                    context.Reports.Attach(report);
                    context.Entry<DbExpenseReport>(report).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    context.Reports.Add(report);
                }
                context.SaveChanges();
                return report;
            }
        }

        public void DeleteCharge(int chargeId)
        {
            using (var context = new ExpensesContext())
            {
                var charge = context.Charges.SingleOrDefault(i => i.ChargeId == chargeId);
                if (charge != null)
                {
                    context.Charges.Remove(charge);
                    context.SaveChanges();
                }
            }
        }

        public void DeleteExpenseReport(int reportId)
        {
            using (var context = new ExpensesContext())
            {
                var report = context.Reports.SingleOrDefault(i => i.ExpenseReportId == reportId);
                if (report != null)
                {
                    context.Reports.Remove(report);
                    context.SaveChanges();
                }
            }
        }

        public void CleanRepository()
        {
            using (var context = new ExpensesContext())
            {
                context.Charges.RemoveRange(context.Charges);
                context.Reports.RemoveRange(context.Reports);
                context.Employees.RemoveRange(context.Employees);

                context.SaveChanges();
            }
        }
    }
}
