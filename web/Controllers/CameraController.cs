using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Camtastic.Web.Models;

namespace Camtastic.Web.Controllers
{
	public class CameraController : Controller
	{
		private readonly CameraModel[] _cameras =
		{
			new CameraModel {Url = "http://10.0.30.1/image.jpg", Name = "FXC-A", Username = "admin", Password = "F00bar00"},
			new CameraModel {Url = "http://10.0.30.2/image.jpg", Name = "FXC-B", Username = "admin", Password = "F00bar00"},
			new CameraModel {Url = "http://10.0.30.3/image.jpg", Name = "FXC-C", Username = "admin", Password = "F00bar00"},
			new CameraModel {Url = "http://10.0.31.1/dms", Name = "PTC-A", Username = "admin", Password = "F00bar00"}
		};

		public ActionResult Index()
		{
			return View(_cameras);
		}

		public async Task<ActionResult> RemoteImage(string name)
		{
			var camera = _cameras.SingleOrDefault(c => c.Name == name);
			if (camera == null)
				return new EmptyResult();

			var stream = await CameraViewer.GetImageStreamAsync(camera);
			return File(stream, "image/jpeg");
		}

		public ActionResult CameraJson()
		{
			return ContentAsJson(_cameras);
		}
	}

	public class CameraViewer
	{
		public static async Task<Stream> GetImageStreamAsync(CameraModel camera)
		{
			var http = new HttpClient();
			http.DefaultRequestHeaders.Authorization = GetAuthenticationHeader(camera);

			return await http.GetStreamAsync(camera.Url);
		}

		private static AuthenticationHeaderValue GetAuthenticationHeader(CameraModel camera)
		{
			var basicAuth = $"{camera.Username}:{camera.Password}";
			var byteArray = Encoding.ASCII.GetBytes(basicAuth);

			return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
		}
	}
}