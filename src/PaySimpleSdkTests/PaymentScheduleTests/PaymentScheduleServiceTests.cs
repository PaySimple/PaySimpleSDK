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

using Moq;
using PaySimpleSdk.Accounts;
using PaySimpleSdk.Customers;
using PaySimpleSdk.Helpers;
using PaySimpleSdk.Models;
using PaySimpleSdk.Payments;
using PaySimpleSdk.PaymentSchedules;
using PaySimpleSdk.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PaySimpleSdkTests.PaymentScheduleTests
{
    [ExcludeFromCodeCoverage]
    public class PaymentScheduleServiceTests
    {
        private readonly IPaySimpleSettings settings;
        private readonly Mock<IValidationService> validationService;
        private readonly Mock<IWebServiceRequest> webServiceRequest;
        private readonly Mock<IServiceFactory> serviceFactory;
        private readonly Mock<IAccountService> accountService;
        private readonly Mock<ICustomerService> customerService;
        private readonly PaymentScheduleService service;

        public PaymentScheduleServiceTests()
        {
            settings = new PaySimpleSettings("1234567890", "UnitTests", "https://unittests.paysimple.com");
            validationService = new Mock<IValidationService>();
            webServiceRequest = new Mock<IWebServiceRequest>();
            accountService = new Mock<IAccountService>();
            customerService = new Mock<ICustomerService>();
            serviceFactory = new Mock<IServiceFactory>();

            accountService.Setup(m => m.CreateAchAccountAsync(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            accountService.Setup(m => m.CreateCreditCardAccountAsync(It.IsAny<CreditCard>())).ReturnsAsync(new CreditCard { Id = 1 });
            customerService.Setup(m => m.CreateCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(new Customer { Id = 1 });
            serviceFactory.Setup(m => m.GetAccountService()).Returns(accountService.Object);
            serviceFactory.Setup(m => m.GetCustomerService()).Returns(customerService.Object);

            service = new PaymentScheduleService(settings, validationService.Object, webServiceRequest.Object, serviceFactory.Object);
        }

        // *************************************************************************************************

        [Fact]
        public async Task CreateRecurringPaymentAsync_Verify_ValidationService_Validate()
        {
            // Arrange
            var recurringPayment = new RecurringPayment();

            webServiceRequest.Setup(m => m.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>()))
                .ReturnsAsync(new Result<RecurringPayment>());

            // Act
            await service.CreateRecurringPaymentAsync(recurringPayment);

            // Assert
            validationService.Verify(m => m.Validate(It.IsAny<RecurringPayment>()));
        }

        [Fact]
        public async Task CreateRecurringPaymentAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var recurringPayment = new RecurringPayment();
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>()))
                .Callback((Uri a, RecurringPayment b) => endpoint = a)
                .ReturnsAsync(new Result<RecurringPayment>());

            // Act
            await service.CreateRecurringPaymentAsync(recurringPayment);

            // Assert
            Assert.Equal(string.Format("{0}/v4/recurringpayment", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task CreateRecurringPaymentAsync_Verify_WebServiceRequest_PostDeserializedAsync()
        {
            // Arrange
            var recurringPayment = new RecurringPayment();

            webServiceRequest.Setup(m => m.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>()))
                .ReturnsAsync(new Result<RecurringPayment>());

            // Act
            await service.CreateRecurringPaymentAsync(recurringPayment);

            // Assert
            webServiceRequest.Verify(m => m.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task CreatePaymentPlanAsync_Verify_ValidationService_Validate()
        {
            // Arrange
            var paymentPlan = new PaymentPlan();

            webServiceRequest.Setup(m => m.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(It.IsAny<Uri>(), It.IsAny<PaymentPlan>()))
                .ReturnsAsync(new Result<PaymentPlan>());

            // Act
            await service.CreatePaymentPlanAsync(paymentPlan);

            // Assert
            validationService.Verify(m => m.Validate(It.IsAny<PaymentPlan>()));
        }

        [Fact]
        public async Task CreatePaymentPlanAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var paymentPlan = new PaymentPlan();
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(It.IsAny<Uri>(), It.IsAny<PaymentPlan>()))
                .Callback((Uri a, PaymentPlan b) => endpoint = a)
                .ReturnsAsync(new Result<PaymentPlan>());

            // Act
            await service.CreatePaymentPlanAsync(paymentPlan);

            // Assert
            Assert.Equal(string.Format("{0}/v4/paymentplan", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task CreatePaymentPlanAsync_Verify_WebServiceRequest_PostDeserializedAsync()
        {
            // Arrange
            var paymentPlan = new PaymentPlan();

            webServiceRequest.Setup(m => m.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(It.IsAny<Uri>(), It.IsAny<PaymentPlan>()))
               .ReturnsAsync(new Result<PaymentPlan>());

            // Act
            await service.CreatePaymentPlanAsync(paymentPlan);

            // Assert
            webServiceRequest.Verify(m => m.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(It.IsAny<Uri>(), It.IsAny<PaymentPlan>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task DeleteRecurringPaymentAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var recurringPaymentId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.DeleteAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.DeleteRecurringPaymentAsync(recurringPaymentId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/recurringpayment/{1}", settings.BaseUrl, recurringPaymentId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task DeleteRecurringPaymentAsync_Verify_WebServiceRequest_DeleteAsync()
        {
            // Arrange
            var recurringPaymentId = 1;

            // Act
            await service.DeleteRecurringPaymentAsync(recurringPaymentId);

            // Assert
            webServiceRequest.Verify(m => m.DeleteAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task DeletePaymentPlanAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var paymentPlanId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.DeleteAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.DeletePaymentPlanAsync(paymentPlanId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/paymentplan/{1}", settings.BaseUrl, paymentPlanId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task DeletePaymentPlanAsync_Verify_WebServiceRequest_DeleteAsync()
        {
            // Arrange
            var paymentPlanId = 1;

            // Act
            await service.DeletePaymentPlanAsync(paymentPlanId);

            // Assert
            webServiceRequest.Verify(m => m.DeleteAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetAllPaymentSchedulesAsync_Uses_Correct_Endpoint()
        {
            // Arrange            
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentSchedule>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentSchedule>> { ResultData = new Meta(), Response = new List<PaymentSchedule>() });

            // Act
            await service.GetAllPaymentSchedulesAsync();

            // Assert
            Assert.Equal(string.Format("{0}/v4/paymentschedule?page=1&pagesize=200", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetAllPaymentSchedulesAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentSchedule>>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<IEnumerable<PaymentSchedule>> { ResultData = new Meta(), Response = new List<PaymentSchedule>() });

            // Act
            await service.GetAllPaymentSchedulesAsync();

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentSchedule>>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetRecurringPaymentsAsync_Uses_Correct_Endpoint()
        {
            // Arrange     
            var recurringPaymentId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetRecurringSchedulePaymentsAsync(recurringPaymentId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/recurringpayment/{1}/payments", settings.BaseUrl, recurringPaymentId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetRecurringPaymentsAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var recurringPaymentId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetRecurringSchedulePaymentsAsync(recurringPaymentId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetRecurringScheduleAsync_Uses_Correct_Endpoint()
        {
            // Arrange     
            var recurringPaymentId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<RecurringPayment>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<RecurringPayment>());

            // Act
            await service.GetRecurringScheduleAsync(recurringPaymentId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/recurringpayment/{1}", settings.BaseUrl, recurringPaymentId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetRecurringScheduleAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var recurringPaymentId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<RecurringPayment>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<RecurringPayment>());

            // Act
            await service.GetRecurringScheduleAsync(recurringPaymentId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<RecurringPayment>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetPaymentPlanPaymentsAsync_Uses_Correct_Endpoint()
        {
            // Arrange     
            var paymentPlanId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentPlanPaymentsAsync(paymentPlanId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/paymentplan/{1}/payments", settings.BaseUrl, paymentPlanId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetPaymentPlanPaymentsAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var paymentPlanId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentPlanPaymentsAsync(paymentPlanId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetPaymentPlanScheduleAsync_Uses_Correct_Endpoint()
        {
            // Arrange     
            var paymentPlanId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentPlan>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentPlan>());

            // Act
            await service.GetPaymentPlanScheduleAsync(paymentPlanId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/paymentplan/{1}", settings.BaseUrl, paymentPlanId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetPaymentPlanScheduleAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var paymentPlanId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentPlan>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<PaymentPlan>());

            // Act
            await service.GetPaymentPlanScheduleAsync(paymentPlanId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<PaymentPlan>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task PauseRecurringPaymentAsync_Uses_Correct_Endpoint()
        {
            // Arrange     
            var recurringPaymentId = 1;
            var endDate = DateTime.Now.AddDays(3);
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.PauseRecurringPaymentAsync(recurringPaymentId, endDate);

            // Assert
            Assert.Equal(string.Format("{0}/v4/recurringpayment/{1}/pause?enddate={2}", settings.BaseUrl, recurringPaymentId, endDate.ToString("yyyy-MM-dd")), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task PauseRecurringPaymentAsync_Verify_WebServiceRequest_PutAsync()
        {
            // Arrange
            var recurringPaymentId = 1;
            var endDate = DateTime.Now.AddDays(3);

            // Act
            await service.PauseRecurringPaymentAsync(recurringPaymentId, endDate);

            // Assert
            webServiceRequest.Verify(m => m.PutAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task PausePaymentPlanAsync_Uses_Correct_Endpoint()
        {
            // Arrange     
            var paymentPlanId = 1;
            var endDate = DateTime.Now.AddDays(3);
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.PausePaymentPlanAsync(paymentPlanId, endDate);

            // Assert
            Assert.Equal(string.Format("{0}/v4/paymentplan/{1}/pause?enddate={2}", settings.BaseUrl, paymentPlanId, endDate.ToString("yyyy-MM-dd")), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task PausePaymentPlanAsync_Verify_WebServiceRequest_PutAsync()
        {
            // Arrange
            var paymentPlanId = 1;
            var endDate = DateTime.Now.AddDays(3);

            // Act
            await service.PausePaymentPlanAsync(paymentPlanId, endDate);

            // Assert
            webServiceRequest.Verify(m => m.PutAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task ResumeRecurringPaymentAsync_Uses_Correct_Endpoint()
        {
            // Arrange     
            var recurringPaymentId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.ResumeRecurringPaymentAsync(recurringPaymentId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/recurringpayment/{1}/resume", settings.BaseUrl, recurringPaymentId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task ResumeRecurringPaymentAsync_Verify_WebServiceRequest_PutAsync()
        {
            // Arrange
            var recurringPaymentId = 1;

            // Act
            await service.ResumeRecurringPaymentAsync(recurringPaymentId);

            // Assert
            webServiceRequest.Verify(m => m.PutAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task ResumePaymentPlanAsync_Uses_Correct_Endpoint()
        {
            // Arrange     
            var paymentPlanId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.ResumePaymentPlanAsync(paymentPlanId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/paymentplan/{1}/resume", settings.BaseUrl, paymentPlanId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task ResumePaymentPlanAsync_Verify_WebServiceRequest_PutAsync()
        {
            // Arrange
            var paymentPlanId = 1;

            // Act
            await service.ResumeRecurringPaymentAsync(paymentPlanId);

            // Assert
            webServiceRequest.Verify(m => m.PutAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task SuspendRecurringPaymentAsync_Uses_Correct_Endpoint()
        {
            // Arrange     
            var recurringPaymentId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.SuspendRecurringPaymentAsync(recurringPaymentId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/recurringpayment/{1}/suspend", settings.BaseUrl, recurringPaymentId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task SuspendRecurringPaymentAsync_Verify_WebServiceRequest_PutAsync()
        {
            // Arrange
            var recurringPaymentId = 1;

            // Act
            await service.SuspendRecurringPaymentAsync(recurringPaymentId);

            // Assert
            webServiceRequest.Verify(m => m.PutAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task SuspendPaymentPlanAsync_Uses_Correct_Endpoint()
        {
            // Arrange     
            var paymentPlanId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.SuspendPaymentPlanAsync(paymentPlanId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/paymentplan/{1}/suspend", settings.BaseUrl, paymentPlanId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task SuspendPaymentPlanAsync_Verify_WebServiceRequest_PutAsync()
        {
            // Arrange
            var paymentPlanId = 1;

            // Act
            await service.SuspendPaymentPlanAsync(paymentPlanId);

            // Assert
            webServiceRequest.Verify(m => m.PutAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task UpdateRecurringPaymentAsync_Verify_ValidationService_Validate()
        {
            // Arrange
            var recurringPayment = new RecurringPayment();

            webServiceRequest.Setup(m => m.PutDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>()))
                .ReturnsAsync(new Result<RecurringPayment>());

            // Act
            await service.UpdateRecurringPaymentAsync(recurringPayment);

            // Assert
            validationService.Verify(m => m.Validate(It.IsAny<RecurringPayment>()));
        }

        [Fact]
        public async Task UpdateRecurringPaymentAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var recurringPayment = new RecurringPayment();
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>()))
                .Callback((Uri a, RecurringPayment b) => endpoint = a)
                .ReturnsAsync(new Result<RecurringPayment>());

            // Act
            await service.UpdateRecurringPaymentAsync(recurringPayment);

            // Assert
            Assert.Equal(string.Format("{0}/v4/recurringpayment", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task UpdateRecurringPaymentAsync_Verify_WebServiceRequest_PutDeserializedAsync()
        {
            // Arrange
            var recurringPayment = new RecurringPayment();

            webServiceRequest.Setup(m => m.PutDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>()))
                .ReturnsAsync(new Result<RecurringPayment>());

            // Act
            await service.UpdateRecurringPaymentAsync(recurringPayment);

            // Assert
            webServiceRequest.Verify(m => m.PutDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>()));
        }

        // *************************************************************************************************  
     
        [Fact]
        public async Task CreateNewCustomerPaymentPlanAsync_Verify_ServiceFactory_GetCustomerService()
        {
            // Arrange
            var customerPaymentPlan = new NewCustomerPaymentPlan<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                PaymentPlan = new PaymentPlan()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(It.IsAny<Uri>(), It.IsAny<PaymentPlan>())).ReturnsAsync(new Result<PaymentPlan> { Response = new PaymentPlan { Id = 1 } });

            // Act
            await service.CreateNewCustomerPaymentPlanAsync(customerPaymentPlan);

            // Assert
            serviceFactory.Verify(m => m.GetCustomerService());
        }

        [Fact]
        public async Task CreateNewCustomerPaymentPlanAsync_Verify_CustomerService_CreateCustomerAsync()
        {
            // Arrange
            var customerPaymentPlan = new NewCustomerPaymentPlan<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                PaymentPlan = new PaymentPlan()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(It.IsAny<Uri>(), It.IsAny<PaymentPlan>())).ReturnsAsync(new Result<PaymentPlan> { Response = new PaymentPlan { Id = 1 } });

            // Act
            await service.CreateNewCustomerPaymentPlanAsync(customerPaymentPlan);

            // Assert
            customerService.Verify(m => m.CreateCustomerAsync(It.IsAny<Customer>()));
        }

        [Fact]
        public async Task CreateNewCustomerPaymentPlanAsync_Result_Has_Ids_Populated()
        {
            // Arrange
            var customerPaymentPlan = new NewCustomerPaymentPlan<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                PaymentPlan = new PaymentPlan()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(It.IsAny<Uri>(), It.IsAny<PaymentPlan>())).ReturnsAsync(new Result<PaymentPlan> { Response = new PaymentPlan { Id = 1 } });

            // Act
            var result = await service.CreateNewCustomerPaymentPlanAsync(customerPaymentPlan);

            // Assert
            Assert.Equal(1, result.Customer.Id);
            Assert.Equal(1, result.Account.Id);
            Assert.Equal(1, result.PaymentPlan.Id);
        }
        
        // *************************************************************************************************  
        
        [Fact]
        public async Task CreateNewAccountPaymentPlanAsync_Verify_ServiceFactory_GetAccountService()
        {
            // Arrange
            var customerPaymentPlan = new NewCustomerPaymentPlan<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                PaymentPlan = new PaymentPlan()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(It.IsAny<Uri>(), It.IsAny<PaymentPlan>())).ReturnsAsync(new Result<PaymentPlan> { Response = new PaymentPlan { Id = 1 } });

            // Act
            await service.CreateNewAccountPaymentPlanAsync(customerPaymentPlan);

            // Assert
            serviceFactory.Verify(m => m.GetAccountService());
        }

        [Fact]
        public async Task CreateNewAccountPaymentAsync_Account_Is_Ach_Verify_AccountService_CreateAccountAsync()
        {
            /// Arrange
            var customerPaymentPlan = new NewCustomerPaymentPlan<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                PaymentPlan = new PaymentPlan()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(It.IsAny<Uri>(), It.IsAny<PaymentPlan>())).ReturnsAsync(new Result<PaymentPlan> { Response = new PaymentPlan { Id = 1 } });

            // Act
            await service.CreateNewAccountPaymentPlanAsync(customerPaymentPlan);

            // Assert
            accountService.Verify(m => m.CreateAccountAsync(It.IsAny<Ach>()));
        }

        [Fact]
        public async Task CreateNewAccountPaymentAsync_Account_Is_CreditCard_Verify_AccountService_CreateAccountAsync()
        {
            // Arrange
            var customerPaymentPlan = new NewCustomerPaymentPlan<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                PaymentPlan = new PaymentPlan()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(It.IsAny<Uri>(), It.IsAny<PaymentPlan>())).ReturnsAsync(new Result<PaymentPlan> { Response = new PaymentPlan { Id = 1 } });

            // Act
            await service.CreateNewAccountPaymentPlanAsync(customerPaymentPlan);

            // Assert
            accountService.Verify(m => m.CreateAccountAsync(It.IsAny<Ach>()));
        }

        // *************************************************************************************************  
        
        [Fact]
        public async Task CreateNewCustomerRecurringPaymentAsync_Verify_ServiceFactory_GetCustomerService()
        {
            // Arrange
            var customerRecurringPayment = new NewCustomerRecurringPayment<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                RecurringPayment = new RecurringPayment()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>())).ReturnsAsync(new Result<RecurringPayment> { Response = new RecurringPayment { Id = 1 } });

            // Act
            await service.CreateNewCustomerRecurringPaymentAsync(customerRecurringPayment);

            // Assert
            serviceFactory.Verify(m => m.GetCustomerService());
        }

        [Fact]
        public async Task CreateNewCustomerRecurringPaymentAsync_Verify_CustomerService_CreateCustomerAsync()
        {
            // Arrange
            var customerRecurringPayment = new NewCustomerRecurringPayment<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                RecurringPayment = new RecurringPayment()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>())).ReturnsAsync(new Result<RecurringPayment> { Response = new RecurringPayment { Id = 1 } });

            // Act
            await service.CreateNewCustomerRecurringPaymentAsync(customerRecurringPayment);

            // Assert
            customerService.Verify(m => m.CreateCustomerAsync(It.IsAny<Customer>()));
        }

        [Fact]
        public async Task CreateNewCustomerRecurringPaymentAsync_Result_Has_Ids_Populated()
        {
            // Arrange
            var customerRecurringPayment = new NewCustomerRecurringPayment<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                RecurringPayment = new RecurringPayment()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>())).ReturnsAsync(new Result<RecurringPayment> { Response = new RecurringPayment { Id = 1 } });

            // Act
            var result = await service.CreateNewCustomerRecurringPaymentAsync(customerRecurringPayment);

            // Assert
            Assert.Equal(1, result.Customer.Id);
            Assert.Equal(1, result.Account.Id);
            Assert.Equal(1, result.RecurringPayment.Id);
        }

        // *************************************************************************************************  

        [Fact]
        public async Task CreateNewAccountRecurringPaymentAsync_Verify_ServiceFactory_GetAccountService()
        {
            // Arrange
            var accountRecurringPayment = new NewAccountRecurringPayment<Ach>
            {
                Account = new Ach() { CustomerId = 1 },
                RecurringPayment = new RecurringPayment()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>())).ReturnsAsync(new Result<RecurringPayment> { Response = new RecurringPayment { Id = 1 } });

            // Act
            await service.CreateNewAccountRecurringPaymentAsync(accountRecurringPayment);

            // Assert
            serviceFactory.Verify(m => m.GetAccountService());
        }

        [Fact]
        public async Task CreateNewAccountRecurringPaymentAsync_Account_Is_Ach_Verify_AccountService_CreateAccountAsync()
        {
            // Arrange
            var accountRecurringPayment = new NewAccountRecurringPayment<Ach>
            {
                Account = new Ach() { CustomerId = 1 },
                RecurringPayment = new RecurringPayment()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>())).ReturnsAsync(new Result<RecurringPayment> { Response = new RecurringPayment { Id = 1 } });

            // Act
            await service.CreateNewAccountRecurringPaymentAsync(accountRecurringPayment);

            // Assert
            accountService.Verify(m => m.CreateAccountAsync(It.IsAny<Ach>()));
        }

        [Fact]
        public async Task CreateNewAccountRecurringPaymentAsync_Account_Is_CreditCard_Verify_AccountService_CreateAccountAsync()
        {
            // Arrange
            var accountRecurringPayment = new NewAccountRecurringPayment<CreditCard>
            {
                Account = new CreditCard() { CustomerId = 1 },
                RecurringPayment = new RecurringPayment()
            };
            accountService.Setup(m => m.CreateAccountAsync<CreditCard>(It.IsAny<CreditCard>())).ReturnsAsync(new CreditCard { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(It.IsAny<Uri>(), It.IsAny<RecurringPayment>())).ReturnsAsync(new Result<RecurringPayment> { Response = new RecurringPayment { Id = 1 } });

            // Act
            await service.CreateNewAccountRecurringPaymentAsync(accountRecurringPayment);

            // Assert
            accountService.Verify(m => m.CreateAccountAsync(It.IsAny<CreditCard>()));
        }       
    }
}