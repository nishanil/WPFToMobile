
using Microsoft.Azure.Mobile.Server;
using MyExpenses.MobileAppService.DataObjects;
using MyExpenses.MobileAppService.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace MyExpenses.MobileAppService.Controllers
{
    // TODO: Uncomment [Authorize] attribute to require user be authenticated to access ExpenseReport(s).
    // [Authorize]
    public class ExpenseReportController : TableController<ExpenseReport>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MyExpenseContext context = new MyExpenseContext();
            DomainManager = new EntityDomainManager<ExpenseReport>(context, Request);
        }

        // GET tables/ExpenseReport
        public IQueryable<ExpenseReport> GetAllExpenseReports()
        {
            return Query();
        }

        // GET tables/ExpenseReport/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ExpenseReport> GetExpenseReport(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ExpenseReport/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ExpenseReport> PatchExpenseReport(string id, Delta<ExpenseReport> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/ExpenseReport
        public async Task<IHttpActionResult> PostExpenseReport(ExpenseReport ExpenseReport)
        {
            ExpenseReport current = await InsertAsync(ExpenseReport);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/ExpenseReport/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteExpenseReport(string id)
        {
            return DeleteAsync(id);
        }
    }
}