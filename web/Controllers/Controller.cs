using System.Text;
using System.Web.Mvc;
using Camtastic.Web.Mvc;

namespace Camtastic.Web.Controllers
{
	public class Controller : System.Web.Mvc.Controller
	{
		protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return new NewtonsoftJsonResult(data, contentType, contentEncoding, behavior);
		}

		protected ActionResult ContentAsJson(object data)
		{
			return new ContentAsJsonResult(data);
		}
	}
}