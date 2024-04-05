using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.Dtos
{
	public class ResetPassword
	{
		[Required(ErrorMessage = "This field is required")]
		public string userId { get; set; }
		[Required, DataType(DataType.Password)]
		public string newPassword { get; set; }
		[Required, DataType(DataType.Password)]
		public string confirmpass { get; set; }
	}
}
