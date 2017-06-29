using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaySimpleSdk.Accounts;
using PaySimpleSdk.Customers;
using PaySimpleSdk.Helpers;
using PaySimpleSdk.Models;
using PaySimpleSdk.Payments;

namespace TestHarness
{
    public class InternationalPaymentsTests
    {
        public InternationalPaymentsTests(IPaySimpleSettings settings)
        {
            Pmnt = new PaymentService(settings);
            Cst = new CustomerService(settings);
            Acc = new AccountService(settings);
        }

        public CustomerService Cst { get; set; }

        public AccountService Acc { get; set; }

        public PaymentService Pmnt { get; set; }

        public void RunTests()
        {
            Domestic();
            Canadian();
            UK();
            Miscellaneous();
        }

        private void Domestic()
        {
            const CountryCode country = CountryCode.US;
            var tests = new List<Task>();
            var testCounter = 0;

            var address = new Address
            {
                Country = country,
                City = "Denver",
                StateCode = StateCode.CO,
                StreetAddress1 = "1414 Wynkoop St",
                ZipCode = "80202"
            };

            tests.Add(ExecutePaymentTest("US card with 5 digit zip and no address", country, "12345", null, ref testCounter));
            tests.Add(ExecutePaymentTest("US card with no zip and valid address", country, null, address, ref testCounter));
            tests.Add(ExecutePaymentTest("US card wtih 9 digit zip and no address", country, "12345-6789", null, ref testCounter));

            Task.WaitAll(tests.ToArray());
        }

        private void Canadian()
        {
            const CountryCode country = CountryCode.CA;
            var tests = new List<Task>();
            var testCounter = 0;

            var billingAddress = new Address
            {
                Country = country,
                City = "Ottowa",
                StreetAddress1 = "234 Laurier Avenue West",
                ZipCode = "K1A 0G9"
            };

            tests.Add(ExecutePaymentTest("Canadian card with 6 character postal code and no address", country, "K1A 0G9", null, ref testCounter));
            tests.Add(ExecutePaymentTest("Canadian card with no postal code and valid address", country, null, billingAddress, ref testCounter));

            Task.WaitAll(tests.ToArray());
        }

        private void UK()
        {
            const CountryCode country = CountryCode.GB;
            var tests = new List<Task>();
            var testCounter = 0;

            var address = new Address
            {
                Country = country,
                City = "London",
                StreetAddress1 = "4 Piccadilly Gardens",
                ZipCode = "M1 1AF"
            };

            tests.Add(ExecutePaymentTest("UK card with 5 character postal code and no address", country, "M1 1AF", null, ref testCounter));
            tests.Add(ExecutePaymentTest("UK card without postal code and valid address", country, null, address, ref testCounter));
            tests.Add(ExecutePaymentTest("UK card with 6 character postal code and no address", country, "W1A 1AA", null, ref testCounter));
            tests.Add(ExecutePaymentTest("UK card with 7 character postal code and no address", country, "SW1W 0PP", null, ref testCounter));

            Task.WaitAll(tests.ToArray());
        }

        private void Miscellaneous()
        {
            var tests = new List<Task>();
            var testCounter = 0;

            tests.Add(ExecutePaymentTest("Danish card with 4 digit postal code and no address", CountryCode.DK, "2100", null, ref testCounter));
            tests.Add(ExecutePaymentTest("Israeli card with 7 digit postal code and no address", CountryCode.IL, "6343229", null, ref testCounter));

            Task.WaitAll(tests.ToArray());
        }

        private Task ExecutePaymentTest(string testName, CountryCode country, string billingZip,
            Address customerBillingAddress, ref int testCounter)
        {
            testCounter++;
            var customer = new Customer
            {
                FirstName = country.ToString(),
                LastName = testCounter.ToString(),
                ShippingSameAsBilling = true,
                BillingAddress = customerBillingAddress
            };

            var card = new CreditCard
            {
                CreditCardNumber = "4111111111111111",
                Issuer = Issuer.Visa,
                ExpirationDate = "12/2020",
                BillingZipCode = billingZip,
            };
            var payment = new Payment
            {
                Amount = 0.10m
            };
            return Task.Factory.StartNew(() =>
            {
                customer = Cst.CreateCustomerAsync(customer).Result;
                card.CustomerId = customer.Id;
                card = Acc.CreateCreditCardAccountAsync(card).Result;
                payment.AccountId = card.Id;
                payment = Pmnt.CreatePaymentAsync(payment).Result;
                if (payment.Id < 1)
                {
                    Console.WriteLine($"Failed to create payment for test: {testName}");
                    throw new InternationPaymentsTestException("Failed to create payment");
                }
                Harness.DumpObject(testName, payment);
            });
        }
    }

    internal class InternationPaymentsTestException : Exception
    {
        public InternationPaymentsTestException(string message) : base(message)
        {
        }
    }
}