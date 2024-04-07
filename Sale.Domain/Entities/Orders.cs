using Sale.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Domain.Entities
{
	public class Orders : AuditableEntity
	{
        public string Status { get; set; }
        public Guid? UserId  { get; set; }
        public double TotalCount { get; set; }
        public double Shipping { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [ForeignKey("CartId")]
        public virtual List<Cart> Carts { get; set; }

    }
}
