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
using PaySimpleSdk.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace PaySimpleSdkTests.PaymentTests
{
    [ExcludeFromCodeCoverage]
    public class PaymentServiceTests
    {
        private readonly IPaySimpleSettings settings;
        private readonly Mock<IValidationService> validationService;
        private readonly Mock<IWebServiceRequest> webServiceRequest;
        private readonly Mock<IServiceFactory> serviceFactory;
        private readonly Mock<IAccountService> accountService;
        private readonly Mock<ICustomerService> customerService;
        private readonly PaymentService service;

        public PaymentServiceTests()
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

            service = new PaymentService(settings, validationService.Object, webServiceRequest.Object, serviceFactory.Object);
        }

        // *************************************************************************************************

        [Fact]
        public async Task CreateCustomerAsync_Verify_ValidationService_Validate()
        {
            // Arrange
            var payment = new Payment();

            webServiceRequest.Setup(m => m.PostDeserializedAsync<Payment, Result<Payment>>(It.IsAny<Uri>(), It.IsAny<Payment>()))
                .ReturnsAsync(new Result<Payment>());

            // Act
            await service.CreatePaymentAsync(payment);

            // Assert
            validationService.Verify(m => m.Validate(It.IsAny<Payment>()));
        }

        [Fact]
        public async Task CreateCustomerAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var payment = new Payment();
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PostDeserializedAsync<Payment, Result<Payment>>(It.IsAny<Uri>(), It.IsAny<Payment>()))
                .Callback((Uri a, Payment b) => endpoint = a)
                .ReturnsAsync(new Result<Payment>());

            // Act
            await service.CreatePaymentAsync(payment);

            // Assert
            Assert.Equal(string.Format("{0}/v4/payment", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task CreateCustomerAsync_Verify_WebServiceRequest_PostDeserializedAsync()
        {
            // Arrange
            var payment = new Payment();

            webServiceRequest.Setup(m => m.PostDeserializedAsync<Payment, Result<Payment>>(It.IsAny<Uri>(), It.IsAny<Payment>()))
                .ReturnsAsync(new Result<Payment>());

            // Act
            await service.CreatePaymentAsync(payment);

            // Assert
            webServiceRequest.Verify(m => m.PostDeserializedAsync<Payment, Result<Payment>>(It.IsAny<Uri>(), It.IsAny<Payment>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetPaymentAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var paymentId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<Payment>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<Payment>());

            // Act
            await service.GetPaymentAsync(paymentId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/payment/{1}", settings.BaseUrl, paymentId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetPaymentAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            var paymentId = 1;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<Payment>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<Payment>());

            // Act
            await service.GetPaymentAsync(paymentId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<Payment>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetPaymentsAsync_Uses_Correct_Base_Endpoint()
        {
            // Arrange                        
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync();

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("{0}/v4/payment", settings.BaseUrl)));
        }

        [Fact]
        public async Task GetPaymentsAsync_StartDate_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange                  
            var startDate = DateTime.Now;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(startDate: startDate);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&startdate={0}", startDate.ToString("yyyy-MM-dd"))));
        }

        [Fact]
        public async Task GetPaymentsAsync_EndDate_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange                  
            var endDate = DateTime.Now;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(endDate: endDate);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains(string.Format("&enddate={0}", endDate.ToString("yyyy-MM-dd"))));
        }

        [Fact]
        public async Task GetPaymentsAsync_Status_Is_Not_Null_Contains_Correct_Fragment()
        {
            // Arrange                  
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
            await service.GetPaymentsAsync(status: status);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&status=authorized,chargeback,failed,pending,posted,refundsettled,returned,reversed,reversensf,reverseposted,settled,voided"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_Is_PaymentId_Does_Not_Contain_SortBy_Fragment()
        {
            // Arrange      
            var sortBy = PaymentSort.PaymentId;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(sortBy: sortBy);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&sortby=paymentid"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_IsActualSettledDate_Contains_Correct_Fragment()
        {
            // Arrange                  
            var sortBy = PaymentSort.ActualSettledDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=actualsettleddate"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_Amount_Contains_Correct_Fragment()
        {
            // Arrange                  
            var sortBy = PaymentSort.Amount;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=amount"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_EstimatedSettleDate_Contains_Correct_Fragment()
        {
            // Arrange                  
            var sortBy = PaymentSort.EstimatedSettleDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=estimatedsettledate"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_PaymentDate_Contains_Correct_Fragment()
        {
            // Arrange                  
            var sortBy = PaymentSort.PaymentDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymentdate"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_PaymentSubType_Contains_Correct_Fragment()
        {
            // Arrange                  
            var sortBy = PaymentSort.PaymentSubType;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymentsubtype"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_PaymentType_Contains_Correct_Fragment()
        {
            // Arrange                  
            var sortBy = PaymentSort.PaymentType;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
               .Callback((Uri a) => endpoint = a)
               .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=paymenttype"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PaymentSort_ReturnDate_Contains_Correct_Fragment()
        {
            // Arrange                  
            var sortBy = PaymentSort.ReturnDate;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(sortBy: sortBy);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&sortby=returndate"));
        }

        [Fact]
        public async Task GetPaymentsAsync_SortDirection_ASC_Does_Not_Contains_Direction_Fragment()
        {
            // Arrange                  
            var direction = SortDirection.ASC;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(direction: direction);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&direction=ASC"));
        }

        [Fact]
        public async Task GetPaymentsAsync_SortDirection_DESC_Contains_Correct_Fragment()
        {
            // Arrange                  
            var direction = SortDirection.DESC;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(direction: direction);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&direction=DESC"));
        }

        [Fact]
        public async Task GetPaymentsAsync_Page_Is_1_Does_Not_Contains_Page_Fragment()
        {
            // Arrange                  
            var page = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(page: page);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&page=1"));

        }

        [Fact]
        public async Task GetPaymentsAsync_Page_Is_2_Contains_Correct_Fragment()
        {
            // Arrange                  
            var page = 2;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(page: page);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&page=2"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PageSize_Is_200_Does_Not_Contains_Page_Fragment()
        {
            // Arrange                  
            var pageSize = 200;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(pageSize: pageSize);

            // Assert
            Assert.False(endpoint.AbsoluteUri.Contains("&pagesize=200"));
        }

        [Fact]
        public async Task GetPaymentsAsync_PageSize_Is_15_Contains_Correct_Fragment()
        {
            // Arrange                  
            var pageSize = 15;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync(pageSize: pageSize);

            // Assert
            Assert.True(endpoint.AbsoluteUri.Contains("&pagesize=15"));
        }

        [Fact]
        public async Task GetPaymentsAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<IEnumerable<Payment>> { ResultData = new Meta(), Response = new List<Payment>() });

            // Act
            await service.GetPaymentsAsync();

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<IEnumerable<Payment>>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task ReversePaymentAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var paymentId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutAsync<Result<Payment>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<Payment>());

            // Act
            await service.ReversePaymentAsync(paymentId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/payment/{1}/reverse", settings.BaseUrl, paymentId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task ReversePaymentAsync_Verify_WebServiceRequest_PutAsync()
        {
            // Arrange
            var paymentId = 1;

            webServiceRequest.Setup(m => m.PutAsync<Result<Payment>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<Payment>());

            // Act
            await service.ReversePaymentAsync(paymentId);

            // Assert
            webServiceRequest.Verify(m => m.PutAsync<Result<Payment>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task VoidPaymentAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var paymentId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutAsync<Result<Payment>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<Payment>());

            // Act
            await service.VoidPaymentAsync(paymentId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/payment/{1}/void", settings.BaseUrl, paymentId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task VoidPaymentAsync_Verify_WebServiceRequest_PutAsync()
        {
            // Arrange
            var paymentId = 1;

            webServiceRequest.Setup(m => m.PutAsync<Result<Payment>>(It.IsAny<Uri>()))
                .ReturnsAsync(new Result<Payment>());

            // Act
            await service.ReversePaymentAsync(paymentId);

            // Assert
            webServiceRequest.Verify(m => m.PutAsync<Result<Payment>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************       

        [Fact]
        public async Task CreateNewCustomerPaymentAsync_Verify_ServiceFactory_GetCustomerService()
        {
            // Arrange
            var customerPayment = new NewCustomerPayment<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                Payment = new Payment()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<Payment, Result<Payment>>(It.IsAny<Uri>(), It.IsAny<Payment>())).ReturnsAsync(new Result<Payment> { Response = new Payment { Id = 1 } });

            // Act
            await service.CreateNewCustomerPaymentAsync(customerPayment);

            // Assert
            serviceFactory.Verify(m => m.GetCustomerService());
        }

        [Fact]
        public async Task CreateNewCustomerPaymentAsync_Verify_CustomerService_CreateCustomerAsync()
        {
            // Arrange          
            var customerPayment = new NewCustomerPayment<Ach>
            {
                Customer = new Customer(),
                Account = new Ach(),
                Payment = new Payment()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<Payment, Result<Payment>>(It.IsAny<Uri>(), It.IsAny<Payment>())).ReturnsAsync(new Result<Payment> { Response = new Payment { Id = 1 } });

            // Act
            await service.CreateNewCustomerPaymentAsync(customerPayment);

            // Assert
            customerService.Verify(m => m.CreateCustomerAsync(It.IsAny<Customer>()));
        }

        [Fact]
        public async Task CreateNewCustomerPaymentAsync_Result_Has_Ids_Populated()
        {
            // Arrange          
            var customerPayment = new NewCustomerPayment<CreditCard>
            {
                Customer = new Customer(),
                Account = new CreditCard(),
                Payment = new Payment()
            };
            accountService.Setup(m => m.CreateAccountAsync<CreditCard>(It.IsAny<CreditCard>())).ReturnsAsync(new CreditCard { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<Payment, Result<Payment>>(It.IsAny<Uri>(), It.IsAny<Payment>())).ReturnsAsync(new Result<Payment> { Response = new Payment { Id = 1 } });


            // Act
            var result = await service.CreateNewCustomerPaymentAsync(customerPayment);

            // Assert
            Assert.Equal(1, result.Customer.Id);
            Assert.Equal(1, result.Account.Id);
            Assert.Equal(1, result.Payment.Id);
        }

        // *************************************************************************************************  

        [Fact]
        public async Task CreateNewAccountPaymentAsync_Verify_ServiceFactory_GetAccountService()
        {
            // Arrange
            var customerPayment = new NewAccountPayment<Ach>
            {
                Account = new Ach() { CustomerId = 1 },
                Payment = new Payment()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<Payment, Result<Payment>>(It.IsAny<Uri>(), It.IsAny<Payment>())).ReturnsAsync(new Result<Payment> { Response = new Payment { Id = 1 } });

            // Act
            await service.CreateNewAccountPaymentAsync(customerPayment);

            // Assert
            serviceFactory.Verify(m => m.GetAccountService());
        }

        [Fact]
        public async Task CreateNewAccountPaymentAsync_Account_Is_Ach_Verify_AccountService_CreateAccountAsync()
        {
            // Arrange          
            var customerPayment = new NewAccountPayment<Ach>
            {
                Account = new Ach() { CustomerId = 1 },
                Payment = new Payment()
            };
            accountService.Setup(m => m.CreateAccountAsync<Ach>(It.IsAny<Ach>())).ReturnsAsync(new Ach { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<Payment, Result<Payment>>(It.IsAny<Uri>(), It.IsAny<Payment>())).ReturnsAsync(new Result<Payment> { Response = new Payment { Id = 1 } });

            // Act
            await service.CreateNewAccountPaymentAsync(customerPayment);

            // Assert
            accountService.Verify(m => m.CreateAccountAsync(It.IsAny<Ach>()));
        }

        [Fact]
        public async Task CreateNewAccountPaymentAsync_Account_Is_CreditCard_Verify_AccountService_CreateAccountAsync()
        {
            // Arrange          
            var customerPayment = new NewAccountPayment<CreditCard>
            {
                Account = new CreditCard() { CustomerId = 1 },
                Payment = new Payment()
            };
            accountService.Setup(m => m.CreateAccountAsync<CreditCard>(It.IsAny<CreditCard>())).ReturnsAsync(new CreditCard { Id = 1 });
            webServiceRequest.Setup(m => m.PostDeserializedAsync<Payment, Result<Payment>>(It.IsAny<Uri>(), It.IsAny<Payment>())).ReturnsAsync(new Result<Payment> { Response = new Payment { Id = 1 } });

            // Act
            await service.CreateNewAccountPaymentAsync(customerPayment);

            // Assert
            accountService.Verify(m => m.CreateAccountAsync(It.IsAny<CreditCard>()));
        }
    }
}