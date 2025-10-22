using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benteler.WorkPlan.Api.SharedModels.Authentication.Dto
{
	/// <summary>
	/// A model representing the register credentials. For api calls.
	/// </summary>
	public class Register(string email, string password)
    {
		public string email { get; set; } = email;
		public string password { get; set; } = password;
	}
}
