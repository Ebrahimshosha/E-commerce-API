﻿using System.ComponentModel.DataAnnotations;

namespace TalabatAPIs.DTO
{
	public class RigisterDto
	{
		[Required]
		public string DisplayName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&amp;*()_+]).*$"
			, ErrorMessage = "Pssword must contain 1 uppercase , 1 lowercase, 1 Digit , 1 Special charachter")]
		public string Password { get; set; }

		[Required]
		[Phone]
		public string PhoneNumber { get; set; }
	}
}
