using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PaySimpleSdk.Accounts
{
	public class ProtectedCardData
	{
		/// <summary>
		/// The data that is read from swiping a card
		/// </summary>
		[JsonProperty]
		public string TrackData { get; set; }

		/// <summary>
		/// The security code found on the back of the card
		/// </summary>
		[JsonProperty]
		public string Cvv { get; set; }
	}
}
