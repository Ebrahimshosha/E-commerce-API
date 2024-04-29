namespace TalabatAPIs.Error
{
	public class ApiRespponse
	{
		public int StatusCode { get; set; }
		public string? Message { get; set; }

		public ApiRespponse(int statuscode, string? message = null)
		{
			StatusCode = statuscode;
			Message = message ?? GetDefaultMessageForStatusCode(statuscode);
		}

		private string? GetDefaultMessageForStatusCode(int? statuscode)
		{
			return statuscode switch
			{
				400 => "Bad Request",
				401 => "U are Not Authorized",
				404 => "Resource Not Found",
				500 => "Internal server Error",
				_ => null
			};
		}
	}
}
