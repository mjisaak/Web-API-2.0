using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebAPI.Library.Filter
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext aActionContext)
        {
            if (!aActionContext.ModelState.IsValid)
            {
                aActionContext.Response = aActionContext.Request.CreateErrorResponse(
                HttpStatusCode.BadRequest, aActionContext.ModelState);
            }
        }
    }
}