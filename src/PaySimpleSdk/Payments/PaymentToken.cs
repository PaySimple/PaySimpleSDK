using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PaySimpleSdk.Payments
{
	public class PaymentToken
	{
		[JsonProperty("PaymentToken", NullValueHandling = NullValueHandling.Ignore)]
		public string Token { get; set; }

		[JsonProperty]
		public int CustomerId { get; set; }

		[JsonProperty]
		public int CustomerAccountId { get; set; }
		
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? IsNewlyCreated { get; set; } 
	}
}
