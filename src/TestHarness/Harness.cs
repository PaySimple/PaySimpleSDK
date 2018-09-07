#region License
// The MIT License (MIT)
//
// Copyright (c) 2015 Scott Lance
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// The most recent version of this license can be found at: http://opensource.org/licenses/MIT
#endregion

using PaySimpleSdk.Accounts;
using PaySimpleSdk.Customers;
using PaySimpleSdk.Exceptions;
using PaySimpleSdk.Helpers;
using PaySimpleSdk.Models;
using PaySimpleSdk.Payments;
using PaySimpleSdk.PaymentSchedules;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TestHarness
{
    public class Harness
    {
        IPaySimpleSettings settings;
        IAccountService accountService;
        ICustomerService customerService;
        IPaymentService paymentService;
        IPaymentScheduleService paymentScheduleService;

        public Harness()
        {
            var apiKey = ConfigurationManager.AppSettings["apiKey"];
            var username = ConfigurationManager.AppSettings["username"];
            settings = new PaySimpleSettings(apiKey, username, "https://sandbox-api.paysimple.com");
            accountService = new AccountService(settings);
            customerService = new CustomerService(settings);
            paymentService = new PaymentService(settings);
            paymentScheduleService = new PaymentScheduleService(settings);
        }

        public async Task RunMethods()
        {
            try
            {
                // Run this to run everything
                await RunTheGauntlet();

                // Run this for PaySimple Certification
                //await Certification();

                var internationalTests = new InternationalPaymentsTests(settings);
                internationalTests.RunTests();
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }
            catch (Exception ex)
            {
                DumpObject("Exception", ex);
            }
        }

        #region Certification

        public async Task Certification()
        {
            var customerId = await CustomerCertification();
            var accounts = await AccountCertification(customerId);
            await PaymentCertification(accounts.AchAccounts.First().Id, accounts.CreditCardAccounts.First().Id);
        }

        #endregion

        #region Gauntlet

        // This method is intended to run all immediately available operations
        // (e.g. operations that can run without waiting for long running system
        // actions like the ability to Refund a Settled payment).
        // WILL YOUR PAYMENT SURVIVE?
        public async Task RunTheGauntlet()
        {
            var warrior = new Customer
            {
                FirstName = "Red",
                LastName = "Warrior",
                Email = "rwarrior@gauntlet.com"
            };

            var valkyrie = new Customer
            {
                FirstName = "Blue",
                LastName = "Valkyrie",
                Email = "bvalkyrie@gauntlet.com"
            };

            var wizard = new Customer
            {
                FirstName = "Yellow",
                LastName = "Wizard",
                Email = "ywizard@gauntlet.com"
            };

            var elf = new Customer
            {
                FirstName = "Green",
                LastName = "Elf",
                Email = "gelf@gauntlet.com"
            };

            // Create Customer
            warrior = await CreateCustomerAsync(warrior);
			valkyrie = await CreateCustomerAsync(valkyrie);
            wizard = await CreateCustomerAsync(wizard);
            elf = await CreateCustomerAsync(elf);

            if (warrior == null || valkyrie == null || wizard == null || elf == null)
            {
                Console.WriteLine("Failed to create customer");
                return;
            }

            // Update Customer
            warrior.Website = "http://www.gauntlet.com";
            warrior.ShippingSameAsBilling = true;
            warrior = await UpdateCustomerAsync(warrior);

            if (warrior.Website != "http://www.gauntlet.com")
            {
                Console.WriteLine("Failed to update customer");
                return;
            }

            // Get Customer 
            var getCustomer = await GetCustomerAsync(warrior.Id);

            if (getCustomer == null)
            {
                Console.WriteLine("Failed to get customer");
                return;
            }

            // Get Customers
            var customers = await GetCustomersAsync();

            if (customers == null || customers.Items == null)
            {
                Console.WriteLine("Failed to get customers");
                return;
            }

            // Create ACH Account
            var ach = new Ach
            {
                CustomerId = warrior.Id,
                RoutingNumber = "131111114",
                AccountNumber = "751111111",
                BankName = "PaySimple Bank"
            };

            ach = await CreateAchAccountAsync(ach);

            if (ach == null)
            {
                Console.WriteLine("Failed to create ACH account");
                return;
            }

            // Create Credit Card Account
            var visa = new CreditCard
            {
                CustomerId = warrior.Id,
                CreditCardNumber = "4111111111111111",
                ExpirationDate = "12/2021",
                Issuer = Issuer.Visa,
                BillingZipCode = "11111"
            };

            visa = await CreateCreditCardAccountAsync(visa);

            if (visa == null)
            {
                Console.WriteLine("Failed to create Credit Card account");
                return;
            }

	        // Create Recurring Payment
	        var recurringPayment2 = new RecurringPayment
	        {
		        AccountId = visa.Id,
		        PaymentAmount = 10.00M,
		        StartDate = DateTime.Now.AddDays(1),
		        ExecutionFrequencyType = ExecutionFrequencyType.FirstOfMonth,
	        };


			// create MC with new bin range
			var newBinMc = new CreditCard
			{
				CustomerId = warrior.Id,
				CreditCardNumber = "2223000048400011",
				ExpirationDate = "12/2021",
				Issuer = Issuer.Master,
				BillingZipCode = "11111"
			};

			newBinMc = await CreateCreditCardAccountAsync(newBinMc);

			if (newBinMc == null)
			{
				Console.WriteLine("Failed to create Credit Card account with new MasterCard bin");
				return;
			}

			// Update Ach Account
			ach.AccountNumber = "751111111";
            ach.IsCheckingAccount = true;
            ach = await UpdateAchAccountAsync(ach);

            if (ach == null)
            {
                Console.WriteLine("Failed to update ACH account");
                return;
            }

            // Update Credit Card Account
            visa.CreditCardNumber = "4111111111111111";
            visa.ExpirationDate = "12/2022";
            visa = await UpdateCreditCardAccountAsync(visa);

            if (visa == null)
            {
                Console.WriteLine("Failed to update Credit Card account");
                return;
            }

            // Get Ach Account
            ach = await GetAchAccountAsync(ach.Id);

            if (ach == null)
            {
                Console.WriteLine("Failed to get ACH account");
                return;
            }

            // Get Credit Card Account
            visa = await GetCreditCardAccountAsync(visa.Id);

            if (visa == null)
            {
                Console.WriteLine("Failed to get Credit Card account");
                return;
            }

            // Match Customer and Credit Card
            visa.CreditCardNumber = "4111111111111111";
            var customerAndCreditCard = new CustomerAndAccountRequest
            {
                Customer = warrior,
                CreditCardAccount = visa
            };

            var matchCreditCardToken = await MatchOrCreateCustomerAndCreditCardAccountAsync(customerAndCreditCard);

            if (string.IsNullOrWhiteSpace(matchCreditCardToken?.Token))
            {
                Console.WriteLine("Failed to match or create customer and credit card");
                return;
            }

            // Match Customer and Ach
            ach.RoutingNumber = "131111114";
            ach.AccountNumber = "751111111";
            var customerAndAch = new CustomerAndAccountRequest
            {
                Customer = warrior,
                AchAccount = ach
            };

            var matchAchToken = await MatchOrCreateCustomerAndAchAccountAsync(customerAndAch);

            if (string.IsNullOrWhiteSpace(matchAchToken?.Token))
            {
                Console.WriteLine("Failed to match or create customer and ach");
                return;
            }

            // Get All Customer Accounts
            var customerAccounts = await GetAllAccountsAsync(warrior.Id);

            if (customerAccounts == null)
            {
                Console.WriteLine("Failed to get customer accounts");
                return;
            }

            // Get Customer Ach Accounts
            var achAccounts = await GetAchAccountsAsync(warrior.Id);

            if (achAccounts == null)
            {
                Console.WriteLine("Failed to get customer ACH accounts");
                return;
            }

            // Get Customer Credit Card Accounts
            var creditCardAccounts = await GetAchAccountsAsync(warrior.Id);

            if (creditCardAccounts == null)
            {
                Console.WriteLine("Failed to get customer Credit Card accounts");
                return;
            }

            // Get Customer Default Ach Account
            var defaultAchAccount = await GetDefaultAchAccountAsync(warrior.Id);

            if (defaultAchAccount == null)
            {
                Console.WriteLine("Failed to get customer default ACH account");
                return;
            }

            // Get Customer Default Credit Card Account
            var defaultCreditCardAccount = await GetDefaultCreditCardAccountAsync(warrior.Id);

            if (defaultCreditCardAccount == null)
            {
                Console.WriteLine("Failed to get customer default Credit Card account");
                return;
            }

            // Set Customer Default Account
            await SetDefaultAccountAsync(warrior.Id, visa.Id);

            // Create Payments

            var tokenRequest = new PaymentTokenRequest
            {
                CustomerAccountId = visa.Id,
                CustomerId = warrior.Id,
                Cvv = "999"
            };

            var paymentToken = await GetPaymentTokenAsync(tokenRequest);

            if (string.IsNullOrWhiteSpace(paymentToken?.Token))
            {
                Console.WriteLine("Failed to get payment token");
                return;
            }

            // Make payment with a payment token
            var tokenPayment = new Payment
            {
                AccountId = visa.Id,
                PaymentToken = paymentToken.Token,
                Amount = 5.00M,
            };

			tokenPayment = await CreatePaymentAsync(tokenPayment);

            if (tokenPayment == null)
            {
                Console.WriteLine("Failed to make payment using token");
                return;
            }

            // Make ach payment
            var achPayment = new Payment
            {
                AccountId = ach.Id,
                Amount = 5.00M
            };

            achPayment = await CreatePaymentAsync(achPayment);

            if (achPayment == null)
            {
                Console.WriteLine("Failed to create ACH payment");
                return;
            }

            // Make credit card payment
            var creditCardPayment = new Payment
            {
                AccountId = visa.Id,
                Amount = 5.00M,
                Cvv = "995",

            };


            creditCardPayment = await CreatePaymentAsync(creditCardPayment);

            if (creditCardPayment == null)
            {
                Console.WriteLine("Failed to create Credit Card payment");
                return;
            }

            // Create Recurring Customer Payment
            var recurringCustomerPayment = new NewCustomerRecurringPayment<CreditCard>
            {
                Customer = new Customer()
                {
                    FirstName = "testrec",
                    LastName = "testreclast",
                    BillingAddress = new Address
                    {
                        StreetAddress1 = "testrecstreet",
                        StreetAddress2 = "Testrecstreet2",
                        City = "testRecCity",
                        StateCode = StateCode.CO,
                        Country = CountryCode.US,
                        ZipCode = "80020"
                    }
                },
                Account = new CreditCard
                {
                    CreditCardNumber = "4111111111111111",
                    ExpirationDate = "12/2022",
                    Issuer = Issuer.Visa
                },
                RecurringPayment = new RecurringPayment
                {
                    PaymentAmount = 25.00M,
                    ExecutionFrequencyType = ExecutionFrequencyType.FirstOfMonth,
                    StartDate = DateTime.Now.AddDays(1)
                }
            };

            var recurringCustomerPaymentFromServer =
                await CreateNewCustomerRecurringPaymentAsync(recurringCustomerPayment);
            if (recurringCustomerPaymentFromServer == null)
            {
                Console.WriteLine("Failed to get recurring customer payment from server");
                return;
            }

            // Get Payment
            creditCardPayment = await GetPaymentAsync(creditCardPayment.Id.Value);

            if (creditCardPayment == null)
            {
                Console.WriteLine("Failed to get Credit Card payment");
                return;
            }

            // Get Payments
            var allPayments = await GetPaymentsAsync();

            if (allPayments == null)
            {
                Console.WriteLine("Failed to get all payments");
                return;
            }

            // Get Customer Payments
            var payments = await GetPaymentsAsync(warrior.Id);

            if (payments == null)
            {
                Console.WriteLine("Failed to get customer payments");
                return;
            }

            // Create Payment Plan
            var paymentPlan = new PaymentPlan
            {
                AccountId = visa.Id,
                TotalDueAmount = 100.00M,
                TotalNumberOfPayments = 5,
                StartDate = DateTime.Now.AddDays(1),
                ExecutionFrequencyType = ExecutionFrequencyType.FirstOfMonth
            };

            paymentPlan = await CreatePaymentPlanAsync(paymentPlan);

            if (paymentPlan == null)
            {
                Console.WriteLine("Failed to create payment plan");
                return;
            }

            // Create Recurring Payment
            var recurringPayment = new RecurringPayment
            {
                AccountId = visa.Id,
                PaymentAmount = 10.00M,
                StartDate = DateTime.Now.AddDays(1),
                ExecutionFrequencyType = ExecutionFrequencyType.FirstOfMonth,
            };

			recurringPayment = await CreateRecurringPaymentAsync(recurringPayment);

            if (recurringPayment == null)
            {
                Console.WriteLine("Failed to create recurring payment");
                return;
            }

            // Update Recurring Payment
            var updateRecurringPayment = new RecurringPayment
            {
                Id = recurringPayment.Id,
                AccountId = visa.Id,
                PaymentAmount = 100.00M,
                StartDate = DateTime.Now.AddDays(1),
                ExecutionFrequencyType = ExecutionFrequencyType.FirstOfMonth
            };

            updateRecurringPayment = await UpdateRecurringPaymentAsync(updateRecurringPayment);

            if (updateRecurringPayment == null)
            {
                Console.WriteLine("Failed to update recurring payment");
                return;
            }

            // Get Recurring Schedule
            recurringPayment = await GetRecurringScheduleAsync(recurringPayment.Id);

            if (recurringPayment == null)
            {
                Console.WriteLine("Failed to get recurring schedule");
                return;
            }

            var exceptionThrown = false;

            try
            {
                await DeleteCustomerAsync(visa.CustomerId);
            }
            catch (PaySimpleEndpointException e)
            {
                exceptionThrown = true;
                Debug.Assert(e.StatusCode == HttpStatusCode.BadRequest, "Status code should equal 400 / 'Bad Request'");
                Debug.Assert(e.EndpointErrors.ResultData.Errors.ErrorMessages.First().Message.Contains(
					"Before you can delete this customer, you must cancel all open recurring payments, payment plans, billing schedules, appointments, future payments and invoices associated with this record."), 
                    "Error message should indicate customer has active recurring payments");
            }

            Debug.Assert(exceptionThrown, "Exception should be thrown indicating customer has recurring payments");

            // Create Recurring Payment ACH
            var recurringPaymentAch = new RecurringPayment
            {
                AccountId = ach.Id,
                PaymentAmount = 10.00M,
                StartDate = DateTime.Now.AddDays(1),
                ExecutionFrequencyType = ExecutionFrequencyType.FirstOfMonth,
                PaymentSubType = PaymentSubType.Tel
            };

            recurringPaymentAch = await CreateRecurringPaymentAsync(recurringPaymentAch);

            if (recurringPaymentAch == null)
            {
                Console.WriteLine("Failed to create ACH recurring payment");
                return;
            }

            if (recurringPaymentAch.PaymentSubType != PaymentSubType.Tel)
            {
                Console.WriteLine("Failed to set payment sub type for ACH recurring payment");
                return;
            }

            // Update Recurring Payment
            var updateRecurringPaymentAch = new RecurringPayment
            {
                Id = recurringPayment.Id,
                AccountId = ach.Id,
                PaymentAmount = 100.00M,
                StartDate = DateTime.Now.AddDays(1),
                ExecutionFrequencyType = ExecutionFrequencyType.FirstOfMonth,
                PaymentSubType = PaymentSubType.Ppd
            };

            updateRecurringPaymentAch = await UpdateRecurringPaymentAsync(updateRecurringPaymentAch);

            if (updateRecurringPaymentAch == null)
            {
                Console.WriteLine("Failed to update ACH recurring payment");
                return;
            }

            if (updateRecurringPaymentAch.PaymentSubType != PaymentSubType.Ppd)
            {
                Console.WriteLine("Failed to update ACH recurring payment sub type");
                return;
            }

            // Get Recurring Schedule
            recurringPaymentAch = await GetRecurringScheduleAsync(recurringPaymentAch.Id);

            if (recurringPaymentAch == null)
            {
                Console.WriteLine("Failed to get ACH recurring schedule");
                return;
            }

            // Get Customer Payment Plans
            var paymentPlans = await GetPaymentPlansAsync(warrior.Id);

            if (paymentPlans == null)
            {
                Console.WriteLine("Failed to get customer payment plans");
                return;
            }

            // Get Customer Payment Schedules
            var paymentSchedules = await GetPaymentSchedulesAsync(warrior.Id);

            if (paymentSchedules == null)
            {
                Console.WriteLine("Failed to get customer payment schedules");
                return;
            }

            // Get Customer Recurring Payment Schedules
            var recurringPaymentSchedules = await GetRecurringPaymentSchedulesAsync(warrior.Id);

            if (recurringPaymentSchedules == null)
            {
                Console.WriteLine("Failed to get customer recurring payment schedules");
                return;
            }

            // Get All Payment Plan Schedules
            var allPaymentSchedules = await GetAllPaymentSchedulesAsync();

            if (allPaymentSchedules == null)
            {
                Console.WriteLine("Failed to get all payment schedules");
                return;
            }

            // Get Payment Plan Payments
            var paymentPlanPayments = await GetPaymentPlanPaymentsAsync(paymentPlan.Id);

            if (paymentPlanPayments == null)
            {
                Console.WriteLine("Failed to get payment plan payments");
                return;
            }

            // Get Payment Plan Schedule
            paymentPlan = await GetPaymentPlanScheduleAsync(paymentPlan.Id);

            if (paymentPlan == null)
            {
                Console.WriteLine("Failed to get payment plan schedule");
                return;
            }

            // Get Recurring Payments
            var recurringPayments = await GetRecurringPaymentsAsync(recurringPayment.Id);

            if (recurringPayments == null)
            {
                Console.WriteLine("Failed to get recurring payments");
                return;
            }

            // Pause Payment Plan
            await PausePaymentPlanAsync(paymentPlan.Id, DateTime.Now.AddDays(1));

            // Pause Recurring Payment
            await PauseRecurringPaymentAsync(recurringPayment.Id, DateTime.Now.AddDays(1));

            // Resume Payment Plan
            await ResumePaymentPlanAsync(paymentPlan.Id);

            // Resume Recurring Payment
            await ResumeRecurringPaymentAsync(recurringPayment.Id);

            // Suspend Payment Plan
            await SuspendPaymentPlanAsync(paymentPlan.Id);

            // Suspend Recurring Payment
            await SuspendRecurringPaymentAsync(recurringPayment.Id);

            // Supend ACH Recurring Payment
            await SuspendRecurringPaymentAsync(recurringPaymentAch.Id);

            // Void Payments
            var voidAchPayment = await VoidPaymentAsync(achPayment.Id.Value);

            if (voidAchPayment == null)
            {
                Console.WriteLine("Failed to void ACH payment");
                return;
            }

            var voidCreditCardPayment = await VoidPaymentAsync(creditCardPayment.Id.Value);

            if (voidCreditCardPayment == null)
            {
                Console.WriteLine("Failed to void Credit Card payment");
                return;
            }

            var voidTokenPayment = await VoidPaymentAsync(tokenPayment.Id.Value);

            if (voidTokenPayment == null)
            {
                Console.WriteLine("Failed to void Payment Token payment");
                return;
            }



			// Delete Payment Plans
			await DeletePaymentPlanAsync(paymentPlan.Id);

            // Delete Recurring Payments
            await DeleteRecurringPaymentAsync(recurringPayment.Id);

            // Delete Accounts
            await DeleteAchAccountAsync(ach.Id);
            await DeleteCreditCardAccountAsync(visa.Id);

            // Delete Customers
            await DeleteCustomerAsync(warrior.Id);
            await DeleteCustomerAsync(valkyrie.Id);
            await DeleteCustomerAsync(wizard.Id);
            await DeleteCustomerAsync(elf.Id);

	        #region Test 204 on customer delete handled

	        //await DeleteCustomerAsync(elf.Id);

	        #endregion
		}

		#endregion

		#region Account Service Methods

		public async Task<AccountList> AccountCertification(int customerId)
        {
            var accounts = new AccountList();

            try
            {
                // Create an ACH account
                var ach = new Ach
                {
                    CustomerId = customerId,
                    RoutingNumber = "307075259",
                    AccountNumber = "751111111",
                    BankName = "PaySimple Bank"
                };

                var createAchResult = await CreateAchAccountAsync(ach);

                // Add for payments certification
                accounts.AchAccounts = new List<Ach> { createAchResult };

                // Sample Error: Attempt to create the same ACH account
                var recreateAchResult = await CreateAchAccountAsync(ach);
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                // 400 Bad Request - Account Already Exists
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            try
            {
                // Create an CreditCard account
                var creditCard = new CreditCard
                {
                    CustomerId = customerId,
                    CreditCardNumber = "371449635398456",
                    ExpirationDate = "12/2021",
                    Issuer = Issuer.Amex
                };

                var createCCResult = await CreateCreditCardAccountAsync(creditCard);

                // Add for payments certification
                accounts.CreditCardAccounts = new List<CreditCard> { createCCResult };

                var invalidCreditCard = new CreditCard
                {
                    CustomerId = customerId,
                    CreditCardNumber = "1111111111111111",
                    ExpirationDate = "12/2021",
                    Issuer = Issuer.Amex
                };

                // Sample Error: Attempt to create the same CreditCard account
                var invalidCCResult = await CreateCreditCardAccountAsync(invalidCreditCard);
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                // 400 Bad Request - Account Already Exists
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return accounts;
        }

        public async Task<T> CreateAccountAsync<T>(T account)
            where T : Account
        {
            var result = await accountService.CreateAccountAsync(account);

            if (result != null)
                DumpObject("CreateAccountAsync", result);

            return result;
        }

        public async Task<Ach> CreateAchAccountAsync(Ach account)
        {
            var result = await accountService.CreateAchAccountAsync(account);

            if (result != null)
                DumpObject("CreateAchAccountAsync", result);

            return result;
        }

        public async Task<CreditCard> CreateCreditCardAccountAsync(CreditCard account)
        {
            var result = await accountService.CreateCreditCardAccountAsync(account);

            if (result != null)
                DumpObject("CreateCreditCardAccountAsync", result);

            return result;
        }

        public async Task DeleteAchAccountAsync(int accountId)
        {
            await accountService.DeleteAchAccountAsync(accountId);
        }

        public async Task DeleteCreditCardAccountAsync(int accountId)
        {
            await accountService.DeleteCreditCardAccountAsync(accountId);
        }

        public async Task<Ach> GetAchAccountAsync(int accountId)
        {
            var result = await accountService.GetAchAccountAsync(accountId);

            if (result != null)
                DumpObject("GetAchAccountAsync", result);

            return result;
        }

        public async Task<CreditCard> GetCreditCardAccountAsync(int accountId)
        {
            var result = await accountService.GetCreditCardAccountAsync(accountId);

            if (result != null)
                DumpObject("GetCreditCardAccountAsync", result);

            return result;
        }

        public async Task<Ach> UpdateAchAccountAsync(Ach account)
        {
            var result = await accountService.UpdateAchAccountAsync(account);

            if (result != null)
                DumpObject("UpdateAchAccountAsync", result);

            return result;
        }

        public async Task<CreditCard> UpdateCreditCardAccountAsync(CreditCard account)
        {
            var result = await accountService.UpdateCreditCardAccountAsync(account);

            if (result != null)
                DumpObject("UpdateCreditCardAccountAsync", result);

            return result;
        }

        #endregion

        #region Customer Service Methods

        public async Task<int> CustomerCertification()
        {
            var customerId = 0;

            try
            {
                // Create Customer
                var customer = new Customer
                {
                    FirstName = "Scott",
                    LastName = "Lance",
                    BillingAddress = new Address
                    {
                        StreetAddress1 = "205 W 700 S",
                        City = "Salt Lake City",
                        StateCode = StateCode.UT,
                        ZipCode = "84101",
                        Country = CountryCode.US
                    }
                };

                var results = await CreateCustomerAsync(customer);
                customerId = results.Id;

                // Create Invalid Customer
                var invalidCustomer = new Customer
                {
                    FirstName = "Scott",
                    LastName = "Lance",
                    BillingAddress = new Address
                    {
                        City = "Salt Lake City",
                        StateCode = StateCode.UT,
                        ZipCode = "84101",
                        Country = CountryCode.US
                    }
                };

                results = await CreateCustomerAsync(invalidCustomer);
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return customerId;
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            var result = await customerService.CreateCustomerAsync(customer);

            if (result != null)
                DumpObject("CreateCustomerAsync", result);

            return result;
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            await customerService.DeleteCustomerAsync(customerId);
        }

        public async Task<IEnumerable<Ach>> GetAchAccountsAsync(int customerId)
        {
            var result = await customerService.GetAchAccountsAsync(customerId);

            if (result != null)
                DumpObject("GetAchAccountsAsync", result);

            return result;
        }

        public async Task<AccountList> GetAllAccountsAsync(int customerId)
        {
            var result = await customerService.GetAllAccountsAsync(customerId);

            if (result != null)
                DumpObject("GetAllAccountsAsync", result);

            return result;
        }

        public async Task<IEnumerable<CreditCard>> GetCreditCardAccountsAsync(int customerId)
        {
            var result = await customerService.GetCreditCardAccountsAsync(customerId);

            if (result != null)
                DumpObject("GetCreditCardAccountsAsync", result);

            return result;
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            var result = await customerService.GetCustomerAsync(customerId);

            if (result != null)
                DumpObject("GetCustomerAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<Customer>>> GetCustomersAsync(CustomerSort sortBy = CustomerSort.LastName, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await customerService.GetCustomersAsync(sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetCustomersAsync", result);

            return result;
        }

        public async Task<Ach> GetDefaultAchAccountAsync(int customerId)
        {
            var result = await customerService.GetDefaultAchAccountAsync(customerId);

            if (result != null)
                DumpObject("GetDefaultAchAccountAsync", result);

            return result;
        }

        public async Task<CreditCard> GetDefaultCreditCardAccountAsync(int customerId)
        {
            var result = await customerService.GetDefaultCreditCardAccountAsync(customerId);

            if (result != null)
                DumpObject("GetDefaultCreditCardAccountAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<PaymentPlan>>> GetPaymentPlansAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, ScheduleStatus? status = null, ScheduleSort sortBy = ScheduleSort.Id, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await customerService.GetPaymentPlansAsync(customerId, startDate, endDate, status, sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetPaymentPlansAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<Payment>>> GetPaymentsAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, IEnumerable<PaymentStatus> status = null, PaymentSort sortBy = PaymentSort.PaymentId, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await customerService.GetPaymentsAsync(customerId, startDate, endDate, status, sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetPaymentsAsync", result);

            return result;
        }

        public async Task<PagedResult<PaymentScheduleList>> GetPaymentSchedulesAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, ScheduleStatus? status = null, ScheduleSort sortBy = ScheduleSort.Id, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await customerService.GetPaymentSchedulesAsync(customerId, startDate, endDate, status, sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetPaymentSchedulesAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<RecurringPayment>>> GetRecurringPaymentSchedulesAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, ScheduleStatus status = ScheduleStatus.Active, ScheduleSort sortBy = ScheduleSort.Id, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await customerService.GetRecurringPaymentSchedulesAsync(customerId, startDate, endDate, status, sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetRecurringPaymentSchedulesAsync", result);

            return result;
        }

        public async Task SetDefaultAccountAsync(int customerId, int accountId)
        {
            await customerService.SetDefaultAccountAsync(customerId, accountId);
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            var result = await customerService.UpdateCustomerAsync(customer);

            if (result != null)
                DumpObject("UpdateCustomerAsync", result);

            return result;
        }

        public async Task<PaymentToken> MatchOrCreateCustomerAndCreditCardAccountAsync(CustomerAndAccountRequest request)
        {
            var result = await customerService.MatchOrCreateCustomerAndCreditCardAccountAsync(request);

            if (result != null)
                DumpObject("MatchOrCreateCustomerAndCreditCardAccountAsync", result);

            return result;
        }

        public async Task<PaymentToken> MatchOrCreateCustomerAndAchAccountAsync(CustomerAndAccountRequest request)
        {
            var result = await customerService.MatchOrCreateCustomerAndAchAccountAsync(request);

            if (result != null)
                DumpObject("MatchOrCreateCustomerAndAchAccountAsync", result);

            return result;
        }

        #endregion

        #region Payment Service Methods

        public async Task PaymentCertification(int achAccountId, int creditCardAccountId)
        {
            try
            {
                var achPayment = new Payment
                {
                    AccountId = achAccountId,
                    Amount = 15.00M,
                    PaymentSubType = PaymentSubType.Web
                };

                // Make a payment
                await CreatePaymentAsync(achPayment);

                // Make the same payment again within 5 minutes
                await CreatePaymentAsync(achPayment);
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            try
            {
                // Make a valid payment
                var creditCardPayment = new Payment
                {
                    AccountId = creditCardAccountId,
                    Amount = 15.00M,
                    Cvv = "996"
                };

                var result = await CreatePaymentAsync(creditCardPayment);

                // Void the payment
                await VoidPaymentAsync(result.Id.Value);

                // Wrong CVV
                var failCreditCardPayment = new Payment
                {
                    AccountId = creditCardAccountId,
                    Amount = 10.00M,
                    PaymentSubType = PaymentSubType.Moto,
                    Cvv = "042"
                };

                // Make a payment with the wrong CVV
                await CreatePaymentAsync(failCreditCardPayment);

                // Zero Amount Payment
                var zeroAmountCreditCardPayment = new Payment
                {
                    AccountId = creditCardAccountId,
                    Amount = 0M,
                    PaymentSubType = PaymentSubType.Moto
                };

                // Make a payment with the wrong CVV
                await CreatePaymentAsync(zeroAmountCreditCardPayment);
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }
        }

        public async Task<NewAccountPayment<T>> CreateNewAccountPaymentAsync<T>(NewAccountPayment<T> accountPayment)
            where T : Account, new()
        {
            try
            {
                var result = await paymentService.CreateNewAccountPaymentAsync<T>(accountPayment);

                if (result != null)
                    DumpObject("CreateNewAccountPaymentAsync", result);

                return result;
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return null;
        }

        public async Task<NewCustomerPayment<T>> CreateNewCustomerPaymentAsync<T>(NewCustomerPayment<T> customerPayment)
            where T : Account, new()
        {
            try
            {
                var result = await paymentService.CreateNewCustomerPaymentAsync<T>(customerPayment);

                if (result != null)
                    DumpObject("CreateNewCustomerPaymentAsync", result);

                return result;
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return null;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            var result = await paymentService.CreatePaymentAsync(payment);

            if (result != null)
                DumpObject("CreatePaymentAsync", result);

            return result;
        }

        public async Task<Payment> GetPaymentAsync(int paymentId)
        {
            var result = await paymentService.GetPaymentAsync(paymentId);

            if (result != null)
                DumpObject("GetPaymentAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<Payment>>> GetPaymentsAsync(DateTime? startDate = null, DateTime? endDate = null, IEnumerable<PaymentStatus> status = null, PaymentSort sortBy = PaymentSort.PaymentId, SortDirection direction = SortDirection.DESC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await paymentService.GetPaymentsAsync(startDate, endDate, status, sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetPaymentsAsync", result);

            return result;
        }

        public async Task<Payment> ReversePaymentAsync(int paymentId)
        {
            var result = await paymentService.ReversePaymentAsync(paymentId);

            if (result != null)
                DumpObject("ReversePaymentAsync", result);

            return result;
        }

        public async Task<Payment> VoidPaymentAsync(int paymentId)
        {
            var result = await paymentService.VoidPaymentAsync(paymentId);

            if (result != null)
                DumpObject("VoidPaymentAsync", result);

            return result;
        }

        public async Task<PaymentToken> GetPaymentTokenAsync(PaymentTokenRequest request)
        {
            var result = await paymentService.GetPaymentTokenAsync(request);

            if (result != null)
                DumpObject("GetPaymentTokenAsync", result);

            return result;
        }

        #endregion

        #region Payment Schedule Service Methods

        public async Task<NewAccountPaymentPlan<T>> CreateNewAccountPaymentPlanAsync<T>(NewAccountPaymentPlan<T> accountPaymentPlan)
            where T : Account, new()
        {
            try
            {
                var result = await paymentScheduleService.CreateNewAccountPaymentPlanAsync<T>(accountPaymentPlan);

                if (result != null)
                    DumpObject("CreateNewAccountPaymentPlanAsync", result);

                return result;
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return null;
        }

        public async Task<NewCustomerPaymentPlan<T>> CreateNewCustomerPaymentPlanAsync<T>(NewCustomerPaymentPlan<T> customerPaymentPlan)
            where T : Account, new()
        {
            try
            {
                var result = await paymentScheduleService.CreateNewCustomerPaymentPlanAsync<T>(customerPaymentPlan);

                if (result != null)
                    DumpObject("CreateNewCustomerPaymentPlanAsync", result);

                return result;
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return null;
        }

        public async Task<PaymentPlan> CreatePaymentPlanAsync(PaymentPlan paymentPlan)
        {
            var result = await paymentScheduleService.CreatePaymentPlanAsync(paymentPlan);

            if (result != null)
                DumpObject("CreatePaymentPlanAsync", result);

            return result;
        }

        public async Task<NewAccountRecurringPayment<T>> CreateNewAccountRecurringPaymentAsync<T>(NewAccountRecurringPayment<T> accountRecurringPayment)
            where T : Account, new()
        {
            try
            {
                var result = await paymentScheduleService.CreateNewAccountRecurringPaymentAsync<T>(accountRecurringPayment);

                if (result != null)
                    DumpObject("CreateNewAccountRecurringPaymentAsync", result);

                return result;
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return null;
        }

        public async Task<NewCustomerRecurringPayment<T>> CreateNewCustomerRecurringPaymentAsync<T>(NewCustomerRecurringPayment<T> customerRecurringPayment)
            where T : Account, new()
        {
            try
            {
                var result = await paymentScheduleService.CreateNewCustomerRecurringPaymentAsync<T>(customerRecurringPayment);

                if (result != null)
                    DumpObject("CreateNewCustomerRecurringPaymentAsync", result);

                return result;
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return null;
        }

        public async Task<RecurringPayment> CreateRecurringPaymentAsync(RecurringPayment recurringPayment)
        {
            var result = await paymentScheduleService.CreateRecurringPaymentAsync(recurringPayment);

            if (result != null)
                DumpObject("CreateRecurringPaymentAsync", result);

            return result;
        }

        public async Task DeletePaymentPlanAsync(int paymentPlanId)
        {
            await paymentScheduleService.DeletePaymentPlanAsync(paymentPlanId);
        }

        public async Task DeleteRecurringPaymentAsync(int recurringPaymentId)
        {
            await paymentScheduleService.DeleteRecurringPaymentAsync(recurringPaymentId);
        }

        public async Task<PagedResult<IEnumerable<PaymentSchedule>>> GetAllPaymentSchedulesAsync()
        {
            var result = await paymentScheduleService.GetAllPaymentSchedulesAsync();

            if (result != null)
                DumpObject("GetAllPaymentSchedulesAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<Payment>>> GetPaymentPlanPaymentsAsync(int paymentPlanId)
        {
            var result = await paymentScheduleService.GetPaymentPlanPaymentsAsync(paymentPlanId);

            if (result != null)
                DumpObject("GetPaymentPlanPaymentsAsync", result);

            return result;
        }

        public async Task<PaymentPlan> GetPaymentPlanScheduleAsync(int paymentPlanId)
        {
            var result = await paymentScheduleService.GetPaymentPlanScheduleAsync(paymentPlanId);

            if (result != null)
                DumpObject("GetPaymentPlanScheduleAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<Payment>>> GetRecurringPaymentsAsync(int recurringPaymentId)
        {
            var result = await paymentScheduleService.GetRecurringSchedulePaymentsAsync(recurringPaymentId);

            if (result != null)
                DumpObject("GetRecurringSchedulePaymentsAsync", result);

            return result;
        }

        public async Task<RecurringPayment> GetRecurringScheduleAsync(int recurringPaymentId)
        {
            var result = await paymentScheduleService.GetRecurringScheduleAsync(recurringPaymentId);

            if (result != null)
                DumpObject("GetRecurringScheduleAsync", result);

            return result;
        }

        public async Task PausePaymentPlanAsync(int paymentPlanId, DateTime endDate)
        {
            await paymentScheduleService.PausePaymentPlanAsync(paymentPlanId, endDate);
        }

        public async Task PauseRecurringPaymentAsync(int recurringPaymentId, DateTime endDate)
        {
            await paymentScheduleService.PauseRecurringPaymentAsync(recurringPaymentId, endDate);
        }

        public async Task ResumePaymentPlanAsync(int paymentPlanId)
        {
            await paymentScheduleService.ResumePaymentPlanAsync(paymentPlanId);
        }

        public async Task ResumeRecurringPaymentAsync(int recurringPaymentId)
        {
            await paymentScheduleService.ResumeRecurringPaymentAsync(recurringPaymentId);
        }

        public async Task SuspendPaymentPlanAsync(int paymentPlanId)
        {
            await paymentScheduleService.SuspendPaymentPlanAsync(paymentPlanId);
        }

        public async Task SuspendRecurringPaymentAsync(int recurringPaymentId)
        {
            await paymentScheduleService.SuspendRecurringPaymentAsync(recurringPaymentId);
        }

        public async Task<RecurringPayment> UpdateRecurringPaymentAsync(RecurringPayment recurringPayment)
        {
            var result = await paymentScheduleService.UpdateRecurringPaymentAsync(recurringPayment);

            if (result != null)
                DumpObject("UpdateRecurringPaymentAsync", result);

            return result;
        }

        #endregion

        #region DumpObject
        public static void DumpObject(string functionName, object obj)
        {
            var sep = "********************************************************************";

            Console.WriteLine(sep);
            Console.WriteLine((functionName + " ").PadRight(sep.Length, '*'));
            Console.WriteLine(sep);

            obj?.PrintDump();

            Console.WriteLine("");
        }
        #endregion
    }
}