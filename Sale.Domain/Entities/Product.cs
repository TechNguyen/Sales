using Sale.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Domain.Entities
{
	[Table("Products")]
	public class Product : AuditableEntity
	{
		public string? BranchId { get; set; }
		public string? CategoryId { get; set;}

		[DisplayName("Tên sản phẩm")]
		public string ProductName { get; set; }
	}
}
