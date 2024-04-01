using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Domain.Entities
{
	[Table("AppUser")]

	public class AppUser : IdentityUser<Guid>
    {
        public string? Address { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
