using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benteler.WorkPlan.Api.SharedModels.Planner;
using Microsoft.AspNetCore.Identity;

namespace Benteler.WorkPlan.Api.SharedModels.Authentication
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the collection of yearly work plans.
        /// </summary>
        public List<Year> WorkPlanForYears { get; set; } = new();
    }
}
