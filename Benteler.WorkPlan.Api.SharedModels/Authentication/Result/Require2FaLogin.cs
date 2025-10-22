using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benteler.WorkPlan.Api.SharedModels.Authentication.Result
{
    /// <summary>
    /// When the login does not succeed because 2FA is required, this result will send to the client in order to login with 2fa
    /// </summary>
    public class Require2FaLogin(string userId, bool requires2Fa = true)
    { 
        public string UserId { get; set; } = userId;
        public bool Requires2Fa { get; set; } = requires2Fa;
    }
}
