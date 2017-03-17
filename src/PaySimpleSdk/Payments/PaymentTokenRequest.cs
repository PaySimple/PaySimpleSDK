using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaySimpleSdk.Accounts;

namespace PaySimpleSdk.Payments
{
	/// <summary>
	/// Used to retrieve a token that will represent the protected card data
	/// </summary>
	public class PaymentTokenRequest : ProtectedCardData
	{
		[JsonProperty]
		public int CustomerAccountId { get; set; }

		[JsonProperty]
		public int CustomerId { get; set; }

		[JsonProperty]
		public bool IsNewlyCreated { get; set; }
	}
}
