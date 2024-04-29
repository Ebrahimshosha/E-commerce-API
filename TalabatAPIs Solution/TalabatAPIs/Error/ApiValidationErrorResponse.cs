namespace TalabatAPIs.Error
{
	public class ApiValidationErrorResponse : ApiRespponse
	{
		public IEnumerable<string> Errors { get; set; }

		public ApiValidationErrorResponse() : base(400)
		{
			Errors = new List<string>();
		}
	}
}
