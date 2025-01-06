using System;
using System.Net;

namespace Repository.Exceptions
{
	public class ExceptionResponse
	{
		public HttpStatusCode StatusCode { get; set; }
		public string Message { get; set; }
		public ExceptionResponse(HttpStatusCode statusCode,string message)
		{
			StatusCode = statusCode;
			Message = message;
		}

	}
}

