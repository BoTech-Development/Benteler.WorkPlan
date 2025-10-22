using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benteler.WorkPlan.Api.SharedModels.Authentication.Dto
{
    public class Enable2Fa(string email, string code)
    {
        public string Email { get; set; } = email;
        public string Code { get; set; } = code;
    }
}
