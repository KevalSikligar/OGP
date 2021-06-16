using System;
using System.Collections.Generic;
using System.Text;

namespace OGP_Portal.Service.Dtos
{
	public class ForcastDto:BaseModel
	{
		public int Quantity { get; set; }
		public long UserId { get; set; }
		public string ForcastDate { get; set; }
		public string ForcastDateStr { get; set; }
	}
}
