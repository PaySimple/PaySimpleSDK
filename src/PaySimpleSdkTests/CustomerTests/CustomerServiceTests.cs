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

namespace PaySimpleSdkTests.CustomerTests
{
    [ExcludeFromCodeCoverage]
    public class CustomerServiceTests
    {
        private readonly IPaySimpleSettings settings;
        private readonly Mock<IValidationService> validationService;
        private readonly Mock<IWebServiceRequest> webServiceRequest;
        private readonly Mock<IServiceFactory> serviceFactory;
        private readonly CustomerService service;

        public CustomerServiceTests()
        {
            settings = new PaySimpleSettings("1234567890", "UnitTests", "https://unittests.paysimple.com");
            validationService = new Mock<IValidationService>();
            webServiceRequest = new Mock<IWebServiceRequest>();
            serviceFactory = new Mock<IServiceFactory>();
            service = new CustomerService(settings, validationService.Object, webServiceRequest.Object, serviceFactory.Object);
        }

        // *************************************************************************************************

        [Fact]
        public async Task CreateCustomerAsync_Verify_ValidationService_Validate()
        {
            // Arrange
            var customer = new Customer();

            webServiceRequest.Setup(m => m.PostDeserializedAsync<Customer, Result<Customer>>(It.IsAny<Uri>(), It.IsAny<Customer>()))
                .ReturnsAsync(new Result<Customer>());

            // Act
            await service.CreateCustomerAsync(customer);

            // Assert
            validationService.Verify(m => m.Validate(It.IsAny<Customer>()));
        }

        [Fact]
        public async Task CreateCustomerAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var customer = new Customer();
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PostDeserializedAsync<Customer, Result<Customer>>(It.IsAny<Uri>(), It.IsAny<Customer>()))
                .Callback((Uri a, Customer b) => endpoint = a)
                .ReturnsAsync(new Result<Customer>());

            // Act
            await service.CreateCustomerAsync(customer);

            // Assert
            Assert.Equal(string.Format("{0}/v4/customer", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task CreateCustomerAsync_Verify_WebServiceRequest_PostDeserializedAsync()
        {
            // Arrange
            var customer = new Customer();

            webServiceRequest.Setup(m => m.PostDeserializedAsync<Customer, Result<Customer>>(It.IsAny<Uri>(), It.IsAny<Customer>()))
                .ReturnsAsync(new Result<Customer>());

            // Act
            await service.CreateCustomerAsync(customer);

            // Assert
            webServiceRequest.Verify(m => m.PostDeserializedAsync<Customer, Result<Customer>>(It.IsAny<Uri>(), It.IsAny<Customer>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task DeleteCustomerAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.DeleteAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.DeleteCustomerAsync(customerId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/customer/{1}", settings.BaseUrl, customerId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task DeleteCustomerAsync_Verify_WebServiceRequest_DeleteAsync()
        {
            // Arrange
            var customerId = 1;

            // Act
            await service.DeleteCustomerAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.DeleteAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetAllAccountsAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<AccountList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<AccountList>());

            // Act
            await service.GetAllAccountsAsync(customerId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/customer/{1}/accounts", settings.BaseUrl, customerId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetAllAccountsAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var customerId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<AccountList>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<AccountList>());

            // Act
            await service.GetAllAccountsAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<AccountList>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetCustomerAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<Customer>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<Customer>());

            // Act
            await service.GetCustomerAsync(customerId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/customer/{1}", settings.BaseUrl, customerId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetCustomerAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var customerId = 1;
            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<Customer>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<Customer>());

            // Act
            await service.GetCustomerAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<Customer>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetCustomersAsync_Uses_Correct_Base_Endpoint()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync();

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("{0}/v4/customer", settings.BaseUrl)));
        }

        [Fact]
        public async Task GetCustomersAsync_Lite_Is_False_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(lite: false);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("?lite=False"));
        }

        [Fact]
        public async Task GetCustomersAsync_Lite_Is_True_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(lite: true);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("?lite=True"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_FirstName_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.FirstName);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=FirstName"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_MiddleName_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.MiddleName);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=MiddleName"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_Company_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.Company);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=Company"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_BillingAddressCity_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.BillingAddressCity);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=BillingAddress.City"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_BillingAddressState_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.BillingAddressState);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=BillingAddress.State"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_BillingAddressZip_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.BillingAddressZip);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=BillingAddress.Zip"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_BillingAddressCountry_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.BillingAddressCountry);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=BillingAddress.Country"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_ShippingAddressCity_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.ShippingAddressCity);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=ShippingAddress.City"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_ShippingAddressState_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.ShippingAddressState);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=ShippingAddress.State"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_ShippingAddressZip_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.ShippingAddressZip);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=ShippingAddress.Zip"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortBy_ShippingAddressCountry_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(sortBy: CustomerSort.ShippingAddressCountry);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=ShippingAddress.Country"));
        }

        [Fact]
        public async Task GetCustomersAsync_SortDirection_DESC_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(direction: SortDirection.DESC);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&direction=DESC"));
        }

        [Fact]
        public async Task GetCustomersAsync_Page_Is_Not_1_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(page: 2);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&page=2"));
        }

        [Fact]
        public async Task GetCustomersAsync_PageSize_Is_Not_200_Contains_Correct_Fragment()
        {
            // Arrange          
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync(pageSize: 15);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&pagesize=15"));
        }

        [Fact]
        public async Task GetCustomersAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()))
               .ReturnsAsync(new Result<IEnumerable<Customer>> { ResultData = new Meta(), Response = new List<Customer>() });

            // Act
            await service.GetCustomersAsync();

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<IEnumerable<Customer>>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetAchAccountsAsync_Uses_Correct_Base_Endpoint()
        {
            // Arrange      
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Ach>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Ach>>());

            // Act
            await service.GetAchAccountsAsync(customerId);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("{0}/v4/customer/{1}/achaccounts", settings.BaseUrl, customerId)));
        }

        [Fact]
        public async Task GetAchAccountsAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var customerId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Ach>>>(It.IsAny<Uri>()))
              .ReturnsAsync(new Result<IEnumerable<Ach>>());

            // Act
            await service.GetAchAccountsAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<IEnumerable<Ach>>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetCreditCardAccountsAsync_Uses_Correct_Base_Endpoint()
        {
            // Arrange      
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<CreditCard>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<CreditCard>>());

            // Act
            await service.GetCreditCardAccountsAsync(customerId);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("{0}/v4/customer/{1}/creditcardaccounts", settings.BaseUrl, customerId)));
        }

        [Fact]
        public async Task GetCreditCardAccountsAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var customerId = 1;
            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<CreditCard>>>(It.IsAny<Uri>()))
               .ReturnsAsync(new Result<IEnumerable<CreditCard>>());

            // Act
            await service.GetCreditCardAccountsAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<IEnumerable<CreditCard>>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetPaymentPlansAsync_Uses_Correct_Base_Endpoint()
        {
            // Arrange            
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("{0}/v4/customer/{1}/paymentplans", settings.BaseUrl, customerId)));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_StartDate_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var startDate = DateTime.Now;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, startDate: startDate);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&startdate={0}", startDate.ToString("yyyy-MM-dd"))));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_EndDate_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var endDate = DateTime.Now;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, endDate: endDate);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&enddate={0}", endDate.ToString("yyyy-MM-dd"))));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_ScheduleStatus_Active_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.Active;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                 .Callback((Uri a) => endpoint = a)
                 .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_ScheduleStatus_Expired_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.Expired;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_ScheduleStatus_PauseUntil_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.PauseUntil;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_ScheduleStatus_Suspended_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.Suspended;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_SortBy_EndDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.EndDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=enddate"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_SortBy_ExecutionFrequencyType_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.ExecutionFrequencyType;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                 .Callback((Uri a) => endpoint = a)
                 .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=executionfrequencytype"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_SortBy_Id_Does_Not_Contains_SortBy_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.Id;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&sortby=id"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_SortBy_NextPaymentDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.NextPaymentDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=nextpaymentdate"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_SortBy_PaymentAmount_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.PaymentAmount;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymentamount"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_SortBy_PaymentScheduleType_Does_Not_Contains_SortBy_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.PaymentScheduleType;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                 .Callback((Uri a) => endpoint = a)
                 .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&sortby=paymentscheduletype"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_SortBy_ScheduleStatus_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.ScheduleStatus;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                 .Callback((Uri a) => endpoint = a)
                 .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=schedulestatus"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_SortBy_StartDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.StartDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=startdate"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_SortDirection_ASC_Does_Not_Contains_Direction_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var direction = SortDirection.ASC;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, direction: direction);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&direction=ASC"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_SortDirection_DESC_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var direction = SortDirection.DESC;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, direction: direction);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&direction=DESC"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_Page_Is_1_Does_Not_Contains_Page_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var page = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, page: page);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&page=1"));

        }

        [Fact]
        public async Task GetPaymentPlansAsync_Page_Is_2_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var page = 2;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, page: page);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&page=2"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_PageSize_Is_200_Does_Not_Contains_Page_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var pageSize = 200;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, pageSize: pageSize);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&pagesize=200"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_PageSize_Is_15_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var pageSize = 15;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId, pageSize: pageSize);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&pagesize=15"));
        }

        [Fact]
        public async Task GetPaymentPlansAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var customerId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<IEnumerable<PaymentPlan>> { ResultData = new Meta(), Response = new List<PaymentPlan>() });

            // Act
            await service.GetPaymentPlansAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<IEnumerable<PaymentPlan>>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetPaymentsAsync_Uses_Correct_Base_Endpoint()
        {
            // Arrange            
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("{0}/v4/customer/{1}/payments", settings.BaseUrl, customerId)));
        }

        [Fact]
        public async Task GetPaymentsAsync_StartDate_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var startDate = DateTime.Now;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, startDate: startDate);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&startdate={0}", startDate.ToString("yyyy-MM-dd"))));
        }

        [Fact]
        public async Task GetPaymentsAsync_EndDate_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var endDate = DateTime.Now;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, endDate: endDate);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&enddate={0}", endDate.ToString("yyyy-MM-dd"))));
        }

        [Fact]
        public async Task GetPaymentsAsync_Status_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var status = new List<PaymentStatus> 
            {
                PaymentStatus.Authorized,
                PaymentStatus.Chargeback,
                PaymentStatus.Failed,
                PaymentStatus.Pending,
                PaymentStatus.Posted,
                PaymentStatus.RefundSettled,
                PaymentStatus.Returned,
                PaymentStatus.Reversed,
                PaymentStatus.ReverseNSF,
                PaymentStatus.ReversePosted,
                PaymentStatus.Settled,
                PaymentStatus.Voided
            };

            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                 .Callback((Uri a) => endpoint = a)
                 .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, status: status);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&status=authorized,chargeback,failed,pending,posted,refundsettled,returned,reversed,reversensf,reverseposted,settled,voided"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_Is_PaymentId_Does_Not_Contain_SortBy_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = PaymentSort.PaymentId;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&sortby=paymentid"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_IsActualSettledDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = PaymentSort.ActualSettledDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=actualsettleddate"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_Amount_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = PaymentSort.Amount;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=amount"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_EstimatedSettleDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = PaymentSort.EstimatedSettleDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=estimatedsettledate"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_PaymentDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = PaymentSort.PaymentDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymentdate"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_PaymentSubType_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = PaymentSort.PaymentSubType;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymentsubtype"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_PaymentType_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = PaymentSort.PaymentType;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymenttype"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_ReturnDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = PaymentSort.ReturnDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=returndate"));
        }

        [Fact]
        public async Task GetPaymentsAsync_SortDirection_ASC_Does_Not_Contains_Direction_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var direction = SortDirection.ASC;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, direction: direction);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&direction=ASC"));
        }

        [Fact]
        public async Task GetPaymentsAsync_SortDirection_DESC_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var direction = SortDirection.DESC;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, direction: direction);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&direction=DESC"));
        }

        [Fact]
        public async Task GetPaymentsAsync_Page_Is_1_Does_Not_Contains_Page_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var page = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, page: page);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&page=1"));

        }

        [Fact]
        public async Task GetPaymentsAsync_Page_Is_2_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var page = 2;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, page: page);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&page=2"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PageSize_Is_200_Does_Not_Contains_Page_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var pageSize = 200;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, pageSize: pageSize);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&pagesize=200"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PageSize_Is_15_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var pageSize = 15;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId, pageSize: pageSize);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&pagesize=15"));
        }

        [Fact]
        public async Task GetPaymentsAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var customerId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetPaymentSchedulesAsync_Uses_Correct_Base_Endpoint()
        {
            // Arrange            
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("{0}/v4/customer/{1}/paymentschedules", settings.BaseUrl, customerId)));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_StartDate_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var startDate = DateTime.Now;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, startDate: startDate);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&startdate={0}", startDate.ToString("yyyy-MM-dd"))));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_EndDate_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var endDate = DateTime.Now;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, endDate: endDate);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&enddate={0}", endDate.ToString("yyyy-MM-dd"))));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_ScheduleStatus_Active_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.Active;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_ScheduleStatus_Expired_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.Expired;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_ScheduleStatus_PauseUntil_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.PauseUntil;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_ScheduleStatus_Suspended_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.Suspended;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_SortBy_EndDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.EndDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=enddate"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_SortBy_ExecutionFrequencyType_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.ExecutionFrequencyType;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=executionfrequencytype"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_SortBy_Id_Does_Not_Contains_SortBy_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.Id;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&sortby=id"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_SortBy_NextPaymentDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.NextPaymentDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=nextpaymentdate"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_SortBy_PaymentAmount_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.PaymentAmount;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymentamount"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_SortBy_PaymentScheduleType_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.PaymentScheduleType;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymentscheduletype"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_SortBy_ScheduleStatus_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.ScheduleStatus;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=schedulestatus"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_SortBy_StartDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.StartDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=startdate"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_SortDirection_ASC_Does_Not_Contains_Direction_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var direction = SortDirection.ASC;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, direction: direction);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&direction=ASC"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_SortDirection_DESC_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var direction = SortDirection.DESC;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                 .Callback((Uri a) => endpoint = a)
                 .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, direction: direction);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&direction=DESC"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_Page_Is_1_Does_Not_Contains_Page_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var page = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, page: page);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&page=1"));

        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_Page_Is_2_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var page = 2;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, page: page);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&page=2"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_PageSize_Is_200_Does_Not_Contains_Page_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var pageSize = 200;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                 .Callback((Uri a) => endpoint = a)
                 .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, pageSize: pageSize);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&pagesize=200"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_PageSize_Is_15_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var pageSize = 15;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId, pageSize: pageSize);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&pagesize=15"));
        }

        [Fact]
        public async Task GetPaymentSchedulesAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var customerId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<PaymentScheduleList> { ResultData = new Meta(), Response = new PaymentScheduleList() });

            // Act
            await service.GetPaymentSchedulesAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<PaymentScheduleList>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_Uses_Correct_Base_Endpoint()
        {
            // Arrange            
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("{0}/v4/customer/{1}/recurringpayments", settings.BaseUrl, customerId)));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_StartDate_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var startDate = DateTime.Now;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, startDate: startDate);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&startdate={0}", startDate.ToString("yyyy-MM-dd"))));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_EndDate_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var endDate = DateTime.Now;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, endDate: endDate);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&enddate={0}", endDate.ToString("yyyy-MM-dd"))));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_ScheduleStatus_Active_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.Active;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_ScheduleStatus_Expired_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.Expired;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_ScheduleStatus_PauseUntil_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.PauseUntil;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_ScheduleStatus_Suspended_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var scheduleStatus = ScheduleStatus.Suspended;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, status: scheduleStatus);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&status={0}", scheduleStatus)));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_SortBy_EndDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.EndDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=enddate"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_SortBy_ExecutionFrequencyType_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.ExecutionFrequencyType;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=executionfrequencytype"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_SortBy_Id_Does_Not_Contains_SortBy_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.Id;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&sortby=id"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_SortBy_NextPaymentDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.NextPaymentDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=nextpaymentdate"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_SortBy_PaymentAmount_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.PaymentAmount;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymentamount"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_SortBy_PaymentScheduleType_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.PaymentScheduleType;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymentscheduletype"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_SortBy_ScheduleStatus_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.ScheduleStatus;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=schedulestatus"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_SortBy_StartDate_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var sortBy = ScheduleSort.StartDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=startdate"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_SortDirection_ASC_Does_Not_Contains_Direction_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var direction = SortDirection.ASC;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                 .Callback((Uri a) => endpoint = a)
                 .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, direction: direction);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&direction=ASC"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_SortDirection_DESC_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var direction = SortDirection.DESC;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, direction: direction);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&direction=DESC"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_Page_Is_1_Does_Not_Contains_Page_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var page = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, page: page);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&page=1"));

        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_Page_Is_2_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var page = 2;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, page: page);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&page=2"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_PageSize_Is_200_Does_Not_Contains_Page_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var pageSize = 200;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, pageSize: pageSize);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&pagesize=200"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_PageSize_Is_15_Contains_Correct_Fragment()
        {
            // Arrange      
            var customerId = 1;
            var pageSize = 15;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId, pageSize: pageSize);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&pagesize=15"));
        }

        [Fact]
        public async Task GetRecurringPaymentSchedulesAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var customerId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<IEnumerable<RecurringPayment>> { ResultData = new Meta(), Response = new List<RecurringPayment>() });

            // Act
            await service.GetRecurringPaymentSchedulesAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetDefaultAchAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<Ach>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<Ach>());

            // Act
            await service.GetDefaultAchAccountAsync(customerId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/customer/{1}/defaultach", settings.BaseUrl, customerId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetDefaultAchAccountAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var customerId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<Ach>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<Ach>());

            // Act
            await service.GetDefaultAchAccountAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<Ach>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetDefaultCreditCardAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var customerId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<CreditCard>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<CreditCard>());

            // Act
            await service.GetDefaultCreditCardAccountAsync(customerId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/customer/{1}/defaultcreditcard", settings.BaseUrl, customerId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetDefaultCreditCardAccountAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var customerId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<CreditCard>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<CreditCard>());

            // Act
            await service.GetDefaultCreditCardAccountAsync(customerId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<CreditCard>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task SetDefaultAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var customerId = 1;
            var accountId = 2;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.SetDefaultAccountAsync(customerId, accountId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/customer/{1}/{2}", settings.BaseUrl, customerId, accountId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task SetDefaultAccountAsync_Verify_WebServiceRequest_PutDeserializedAsync()
        {
            // Arrange
            var customerId = 1;
            var accountId = 2;

            // Act
            await service.SetDefaultAccountAsync(customerId, accountId);

            // Assert
            webServiceRequest.Verify(m => m.PutAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task UpdateCustomerAsync_Verify_ValidationService_Validate()
        {
            // Arrange
            var customer = new Customer();

            webServiceRequest.Setup(m => m.PutDeserializedAsync<Customer, Result<Customer>>(It.IsAny<Uri>(), It.IsAny<Customer>()))
               .ReturnsAsync(new Result<Customer>());

            // Act
            await service.UpdateCustomerAsync(customer);

            // Assert
            validationService.Verify(m => m.Validate(It.IsAny<Customer>()));
        }

        [Fact]
        public async Task UpdateCustomerAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var customer = new Customer();
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutDeserializedAsync<Customer, Result<Customer>>(It.IsAny<Uri>(), It.IsAny<Customer>()))
                .Callback((Uri a, Customer b) => endpoint = a)
                .ReturnsAsync(new Result<Customer>());

            // Act
            await service.UpdateCustomerAsync(customer);

            // Assert
            Assert.Equal(string.Format("{0}/v4/customer", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task UpdateCustomerAsync_Verify_WebServiceRequest_PostDeserializedAsync()
        {
            // Arrange
            var customer = new Customer();

            webServiceRequest.Setup(m => m.PutDeserializedAsync<Customer, Result<Customer>>(It.IsAny<Uri>(), It.IsAny<Customer>()))
                .ReturnsAsync(new Result<Customer>());

            // Act
            await service.UpdateCustomerAsync(customer);

            // Assert
            webServiceRequest.Verify(m => m.PutDeserializedAsync<Customer, Result<Customer>>(It.IsAny<Uri>(), It.IsAny<Customer>()));
        }
    }
}