using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benteler.WorkPlan.Api.SharedModels.Authentication.Dto
{
	/// <summary>
	/// A model representing the email for resending the confirmation email. For api calls.
	/// </summary>
	public class ResendConfirmationEmailModel (string email)
    {
		public string email { get; set; } = email;
	}
}
