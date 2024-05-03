using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.Dtos.OrdersDto
{
	public class SastisDto
	{
        public int? orderSuccess { get; set; }
        public int? orderCancle { get; set; }
        public int? orderFailed { get; set; }
        public int? dataProduct { get; set; }

        public int? dataCountBranchs { get; set; }

        public int? dataCountOrigins { get; set; }

    }
}
