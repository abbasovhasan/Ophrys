﻿using Microsoft.AspNetCore.Identity;

namespace Codeland_Admin_Panel.Models;

using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
