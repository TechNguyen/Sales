using Sale.Domain.Entities;
using Sale.Service.Common;
using Sale.Service.Constant;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Model.Orders
{
	public class OrdersDto
	{
		public Guid Id { get; set; }
		public Guid? UserId { get; set; }

		public DateTime? Createat { get; set; }
		public string Status => ConstantExtension.GetDisPlayConstant<OrdersConstant>(this.Status);
		public string lastName { get; set; }
		public string firstName { get; set; }
		public double totalPrice { get; set; }
		public DateTime? ShippingDate { get; set; }
		public string address { get; set; }
		public string mobile { get; set; }
		public string email { get; set; }
		public string orderNotes { get; set; }
		[ForeignKey("CartId")]
		public virtual List<Cart> Carts { get; set; }

	}
}
