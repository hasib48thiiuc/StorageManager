using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Entities;

namespace StudentManagementSystem.Models.Domain
{
    public class SMSDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public SMSDbContext(DbContextOptions<SMSDbContext> opts) : base(opts)
        {

        }

    }
}
