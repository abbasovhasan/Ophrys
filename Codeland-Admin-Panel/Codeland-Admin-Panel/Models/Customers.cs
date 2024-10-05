using Microsoft.AspNetCore.Identity;

namespace Codeland_Admin_Panel.Models;

public class GetCustomerDetailDto
{
    public class CustomerDetail
    {
        public string FirstName { get; set; }
        public object LastName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}