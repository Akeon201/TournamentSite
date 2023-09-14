using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserRegistration;

namespace UserRegistration
{
    public class Users : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
    }
}


public class IdentityUser : IdentityUser<string>
{
    public IdentityUser();
    public IdentityUser(string userName);
}


