namespace TalabatAPIs.Error
{
	public class ApiExceptionResponse : ApiRespponse
	{
		public string? Details { get; set; }

		public ApiExceptionResponse(int Statuscode, string? Message = null, string? details=null) : base(Statuscode, Message)
		{
			Details = details; 
		}
	}
}
