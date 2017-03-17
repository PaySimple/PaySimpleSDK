using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PaySimpleSdk.Payments
{
	public class CheckoutToken
	{
		[JsonProperty]
		public string JwtToken { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? Expiration { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? IsValid { get; set; }
	}
}
