using Microsoft.AspNetCore.Identity;
using System;

namespace StudentManagementSystem.Entities
{ 
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? UserId { get; set; }
        public string? FullName { get; set; }

        public string? FolderPath { get; set; } 
    }
}
