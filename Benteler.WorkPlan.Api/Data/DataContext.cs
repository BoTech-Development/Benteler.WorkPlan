using Benteler.WorkPlan.Api.SharedModels.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Benteler.WorkPlan.Api.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }
    }
}
