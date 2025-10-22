using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benteler.WorkPlan.Api.SharedModels.Authentication.Result
{

    public class Generated2FaInfo
    {
        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// The private key of the 2fa
        /// </summary>
        public string Key { get; set; } = string.Empty;
        /// <summary>
        /// Provider name in this case "Benetler.WorkPlan
        /// </summary>
        public string Issuer { get; set; } = string.Empty;
        /// <summary>
        /// The uri for the qr code for the Authenticator app.
        /// </summary>
        public string Uri { get; set; } = string.Empty;
    }
}
