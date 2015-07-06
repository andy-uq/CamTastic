using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace web.Controllers
{
	public class CameraController : Controller
	{
		// GET: Camera
		public ActionResult Index()
		{
			return View();
		}

		public async Task<ActionResult> RemoteImage(int index)
		{
			var http = new HttpClient();
			var byteArray = Encoding.ASCII.GetBytes("admin:F00bar00");
			http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

			var urls = new[]
			{
				"http://10.0.30.1/image.jpg",
				"http://10.0.30.2/image.jpg",
				"http://10.0.30.3/image.jpg",
				"http://10.0.31.1/dms"
			};

			var stream = await http.GetStreamAsync(urls[index - 1]);

			return File(stream, "image/jpeg");
		}

		public ActionResult CameraJson()
		{
			var urls = new[]
			{
				new { url = "http://10.0.30.1/image.jpg", name = "FXC-A" },
				new { url = "http://10.0.30.2/image.jpg", name = "FXC-B" },
				new { url = "http://10.0.30.3/image.jpg", name = "FXC-C" },
				new { url = "http://10.0.31.1/dms", name = "PTC-A" }
			};

			return Content(JsonConvert.SerializeObject(urls));
		}
	}
}