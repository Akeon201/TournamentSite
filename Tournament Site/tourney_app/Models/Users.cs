using System;
using Microsoft.AspNetCore.Identity;

namespace team_management_app.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

    }
}
