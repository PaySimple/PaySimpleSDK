using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PaySimpleSdk.Payments
{
	public class PaymentTokenRequest
	{
		[JsonProperty]
		public int CustomerAccountId { get; set; }

		[JsonProperty]
		public int CustomerId { get; set; }

		[JsonProperty]
		public bool IsNewlyCreated { get; set; }

		[JsonProperty]
		public string TrackData { get; set; }

		[JsonProperty]
		public string Cvv { get; set; }
	}
}
