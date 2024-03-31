﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Domain.Entities
{
	[Table("AppRole")]
	public class AppRole : IdentityRole<Guid>
	{
	}
}
