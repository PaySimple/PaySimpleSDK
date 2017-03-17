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
		[JsonProperty]
		public string TrackData { get; set; }

		[JsonProperty]
		public string Cvv { get; set; }
	}
}
