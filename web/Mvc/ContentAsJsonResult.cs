using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Camtastic.Web.Mvc
{
	public class ContentAsJsonResult : ContentResult
	{
		public ContentAsJsonResult(object data)
		{
			Data = data;
		}

		private object Data { get; }

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			var response = context.HttpContext.Response;

			if (Data == null)
				return;

			var serializer = new JsonSerializer();
			serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
			serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;

			using (var writer = new JsonTextWriter(response.Output))
				serializer.Serialize(writer, Data);
		}
	}
}