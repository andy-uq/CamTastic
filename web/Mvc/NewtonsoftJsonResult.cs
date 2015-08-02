using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Camtastic.Web.Mvc
{
	public class NewtonsoftJsonResult : JsonResult
	{
		public NewtonsoftJsonResult(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			ContentEncoding = contentEncoding;
			ContentType = contentType;
			Data = data;
			JsonRequestBehavior = behavior;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
				throw new InvalidOperationException("GET action blocked");

			var response = context.HttpContext.Response;

			response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

			if (ContentEncoding != null)
				response.ContentEncoding = ContentEncoding;

			if (Data == null)
				return;

			var scriptSerializer = new JsonSerializer();
			scriptSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
			scriptSerializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;

			using (var writer = new JsonTextWriter(response.Output))
				scriptSerializer.Serialize(writer, Data);
		}
	}
}