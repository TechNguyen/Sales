using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.Dtos.CommentDto
{
	public class CommentsDto
	{
		public Guid? UserId { get; set; }
		public Guid? ProductId { get; set; }
		public string Comment { get; set; }
        public DateTime createAt { get; set; }
    }
}
