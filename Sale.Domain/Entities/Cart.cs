﻿using Sale.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Domain.Entities
{
	public class Cart : AuditableEntity
	{
        public int? Quanlity { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? PromotionId { get; set; }

    }
}
