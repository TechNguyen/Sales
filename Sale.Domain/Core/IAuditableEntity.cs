using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Domain.Core
{
	public interface IAuditableEntity
	{
		DateTime CreateDate { get; set; }
		string? CreateBy { get; set; }
		Guid? CreatedID {  get; set; }


		DateTime UpdateDate { get; set; }
		Guid? UpdateID { get; set; }
		string? UpdateBy {  get; set; }


		bool? isDelete { get;set; }
		DateTime DeleteDate { get; set; }
		Guid? DeleteID { get; set; }
		string DeleteBy { get; set; }
	}
}
