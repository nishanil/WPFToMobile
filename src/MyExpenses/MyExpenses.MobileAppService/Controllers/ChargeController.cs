
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
    // TODO: Uncomment [Authorize] attribute to require user be authenticated to access Charge(s).
    // [Authorize]
    public class ChargeController : TableController<Charge>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MyExpenseContext context = new MyExpenseContext();
            DomainManager = new EntityDomainManager<Charge>(context, Request);
        }

        // GET tables/Charge
        public IQueryable<Charge> GetAllCharges()
        {
            return Query();
        }

        // GET tables/Charge/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Charge> GetCharge(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Charge/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Charge> PatchCharge(string id, Delta<Charge> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Charge
        public async Task<IHttpActionResult> PostCharge(Charge Charge)
        {
            Charge current = await InsertAsync(Charge);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Charge/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteCharge(string id)
        {
            return DeleteAsync(id);
        }
    }
}