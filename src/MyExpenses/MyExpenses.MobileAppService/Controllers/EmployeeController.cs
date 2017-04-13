
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
    // TODO: Uncomment [Authorize] attribute to require user be authenticated to access Employee(s).
    // [Authorize]
    public class EmployeeController : TableController<Employee>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MyExpenseContext context = new MyExpenseContext();
            DomainManager = new EntityDomainManager<Employee>(context, Request);
        }

        // GET tables/Employee
        public IQueryable<Employee> GetAllEmployees()
        {
            return Query();
        }

        // GET tables/Employee/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Employee> GetEmployee(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Employee/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Employee> PatchEmployee(string id, Delta<Employee> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Employee
        public async Task<IHttpActionResult> PostEmployee(Employee Employee)
        {
            Employee current = await InsertAsync(Employee);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Employee/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteEmployee(string id)
        {
            return DeleteAsync(id);
        }
    }
}