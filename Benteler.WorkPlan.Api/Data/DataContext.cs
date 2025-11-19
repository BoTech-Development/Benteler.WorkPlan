using Benteler.WorkPlan.Api.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Benteler.WorkPlan.Api.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Role> Roles { get; set; }
		public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }
    }
}
