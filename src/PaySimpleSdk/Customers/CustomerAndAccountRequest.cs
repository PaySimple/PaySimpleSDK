using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySimpleSdk.Accounts;

namespace PaySimpleSdk.Customers
{
	public class CustomerAndAccountRequest
	{
		public Customer Customer { get; set; }
		public Ach AchAccount { get; set; }
		public CreditCard CreditCardAccount { get; set; }
		public ProtectedCardData ProtectedCardData { get; set; }
	}
}
