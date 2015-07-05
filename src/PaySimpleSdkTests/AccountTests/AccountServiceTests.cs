#region License
// The MIT License (MIT)
//
// Copyright (c) 2015 Scott Lance, Ethan Tipton
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
using PaySimpleSdk.Helpers;
using PaySimpleSdk.Models;
using PaySimpleSdk.Validation;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PaySimpleSdkTests.AccountTests
{
    [ExcludeFromCodeCoverage]
    public class AccountServiceTests
    {
        private readonly IPaySimpleSettings settings;
        private readonly Mock<IValidationService> validationService;
        private readonly Mock<IWebServiceRequest> webServiceRequest;
        private readonly Mock<IServiceFactory> serviceFactory;
        private readonly AccountService service;

        public AccountServiceTests()
        {
            settings = new PaySimpleSettings("1234567890", "UnitTests", "https://unittests.paysimple.com");
            validationService = new Mock<IValidationService>();
            webServiceRequest = new Mock<IWebServiceRequest>();
            serviceFactory = new Mock<IServiceFactory>();
            service = new AccountService(settings, validationService.Object, webServiceRequest.Object, serviceFactory.Object);
        }

        // *************************************************************************************************

        [Fact]
        public async Task CreateAchAccountAsync_Verify_ValidationService_Validate()
        {
            // Arrange
            webServiceRequest.Setup(m => m.PostDeserializedAsync<Ach, Result<Ach>>(It.IsAny<Uri>(), It.IsAny<Ach>())).ReturnsAsync(new Result<Ach> { Response = new Ach() });
            var ach = new Ach();

            // Act
            await service.CreateAchAccountAsync(ach);

            // Assert
            validationService.Verify(m => m.Validate(It.IsAny<Ach>()));
        }

        [Fact]
        public async Task CreateAchAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var ach = new Ach();
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PostDeserializedAsync<Ach, Result<Ach>>(It.IsAny<Uri>(), It.IsAny<Ach>()))
                .Callback((Uri a, Ach b) => endpoint = a)
                .ReturnsAsync(new Result<Ach>());

            // Act
            await service.CreateAchAccountAsync(ach);

            // Assert
            Assert.Equal(string.Format("{0}/v4/account/ach", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task CreateAchAccountAsync_Verify_WebServiceRequest_PostDeserializedAsync()
        {
            // Arrange
            webServiceRequest.Setup(m => m.PostDeserializedAsync<Ach, Result<Ach>>(It.IsAny<Uri>(), It.IsAny<Ach>())).ReturnsAsync(new Result<Ach> { Response = new Ach() });

            var ach = new Ach();

            // Act
            await service.CreateAchAccountAsync(ach);

            // Assert
            webServiceRequest.Verify(m => m.PostDeserializedAsync<Ach, Result<Ach>>(It.IsAny<Uri>(), It.IsAny<Ach>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task CreateCreditCardAccountAsync_Verify_ValidationService_Validate()
        {
            // Arrange
            webServiceRequest.Setup(m => m.PostDeserializedAsync<CreditCard, Result<CreditCard>>(It.IsAny<Uri>(), It.IsAny<CreditCard>())).ReturnsAsync(new Result<CreditCard> { Response = new CreditCard() });
            var cc = new CreditCard();

            // Act
            await service.CreateCreditCardAccountAsync(cc);

            // Assert
            validationService.Verify(m => m.Validate(It.IsAny<CreditCard>()));
        }

        [Fact]
        public async Task CreateCreditCardAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var cc = new CreditCard();
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PostDeserializedAsync<CreditCard, Result<CreditCard>>(It.IsAny<Uri>(), It.IsAny<CreditCard>()))
                .Callback((Uri a, CreditCard b) => endpoint = a)
                .ReturnsAsync(new Result<CreditCard>());

            // Act
            await service.CreateCreditCardAccountAsync(cc);

            // Assert
            Assert.Equal(string.Format("{0}/v4/account/creditcard", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task CreateCreditCardAccountAsync_Verify_WebServiceRequest_PostDeserializedAsync()
        {
            // Arrange
            webServiceRequest.Setup(m => m.PostDeserializedAsync<CreditCard, Result<CreditCard>>(It.IsAny<Uri>(), It.IsAny<CreditCard>())).ReturnsAsync(new Result<CreditCard> { Response = new CreditCard() });
            var cc = new CreditCard();

            // Act
            await service.CreateCreditCardAccountAsync(cc);

            // Assert
            webServiceRequest.Verify(m => m.PostDeserializedAsync<CreditCard, Result<CreditCard>>(It.IsAny<Uri>(), It.IsAny<CreditCard>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task DeleteAchAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var accountId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.DeleteAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.DeleteAchAccountAsync(accountId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/account/ach/{1}", settings.BaseUrl, accountId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task DeleteAchAccountAsync_Verify_WebServiceRequest_DeleteAsync()
        {
            // Arrange
            var accountId = 1;

            // Act
            await service.DeleteAchAccountAsync(accountId);

            // Assert
            webServiceRequest.Verify(m => m.DeleteAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task DeleteCreditCardAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var accountId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.DeleteAsync(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new HttpResponseMessage());

            // Act
            await service.DeleteCreditCardAccountAsync(accountId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/account/creditcard/{1}", settings.BaseUrl, accountId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task DeleteCreditCardAccountAsync_Verify_WebServiceRequest_DeleteAsync()
        {
            // Arrange
            var accountId = 1;

            // Act
            await service.DeleteCreditCardAccountAsync(accountId);

            // Assert
            webServiceRequest.Verify(m => m.DeleteAsync(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetAchAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            webServiceRequest.Setup(m => m.PostDeserializedAsync<Ach, Result<Ach>>(It.IsAny<Uri>(), It.IsAny<Ach>())).ReturnsAsync(new Result<Ach> { Response = new Ach() });
            var accountId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<Ach>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<Ach>());

            // Act
            await service.GetAchAccountAsync(accountId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/account/ach/{1}", settings.BaseUrl, accountId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetAchAccountAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<Ach>>(It.IsAny<Uri>())).ReturnsAsync(new Result<Ach> { Response = new Ach() });
            var accountId = 1;

            // Act
            await service.GetAchAccountAsync(accountId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<Ach>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task GetCreditCardAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var accountId = 1;
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<CreditCard>>(It.IsAny<Uri>()))
                .Callback((Uri a) => endpoint = a)
                .ReturnsAsync(new Result<CreditCard>());

            // Act
            await service.GetCreditCardAccountAsync(accountId);

            // Assert
            Assert.Equal(string.Format("{0}/v4/account/creditcard/{1}", settings.BaseUrl, accountId), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task GetCreditCardAccountAsync_Verify_WebServiceRequest_GetDeserializedAsync()
        {
            // Arrange
            webServiceRequest.Setup(m => m.GetDeserializedAsync<Result<CreditCard>>(It.IsAny<Uri>())).ReturnsAsync(new Result<CreditCard> { Response = new CreditCard() });
            var accountId = 1;

            // Act
            await service.GetCreditCardAccountAsync(accountId);

            // Assert
            webServiceRequest.Verify(m => m.GetDeserializedAsync<Result<CreditCard>>(It.IsAny<Uri>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task UpdateAchAccountAsync_Verify_ValidationService_Validate()
        {
            // Arrange
            webServiceRequest.Setup(m => m.PutDeserializedAsync<Ach, Result<Ach>>(It.IsAny<Uri>(), It.IsAny<Ach>())).ReturnsAsync(new Result<Ach> { Response = new Ach() });
            var ach = new Ach();

            // Act
            await service.UpdateAchAccountAsync(ach);

            // Assert
            validationService.Verify(m => m.Validate(It.IsAny<Ach>()));
        }

        [Fact]
        public async Task UpdateAchAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var ach = new Ach();
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutDeserializedAsync<Ach, Result<Ach>>(It.IsAny<Uri>(), It.IsAny<Ach>()))
                .Callback((Uri a, Ach b) => endpoint = a)
                .ReturnsAsync(new Result<Ach>());

            // Act
            await service.UpdateAchAccountAsync(ach);

            // Assert
            Assert.Equal(string.Format("{0}/v4/account/ach", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task UpdateAchAccountAsync_Verify_WebServiceRequest_PutDeserializedAsync()
        {
            // Arrange
            webServiceRequest.Setup(m => m.PutDeserializedAsync<Ach, Result<Ach>>(It.IsAny<Uri>(), It.IsAny<Ach>())).ReturnsAsync(new Result<Ach> { Response = new Ach() });
            var ach = new Ach();

            // Act
            await service.UpdateAchAccountAsync(ach);

            // Assert
            webServiceRequest.Verify(m => m.PutDeserializedAsync<Ach, Result<Ach>>(It.IsAny<Uri>(), It.IsAny<Ach>()));
        }

        // *************************************************************************************************

        [Fact]
        public async Task UpdateCreditCardAccountAsync_Verify_ValidationService_Validate()
        {
            // Arrange
            webServiceRequest.Setup(m => m.PutDeserializedAsync<CreditCard, Result<CreditCard>>(It.IsAny<Uri>(), It.IsAny<CreditCard>())).ReturnsAsync(new Result<CreditCard> { Response = new CreditCard() });
            var cc = new CreditCard();

            // Act
            await service.UpdateCreditCardAccountAsync(cc);

            // Assert
            validationService.Verify(m => m.Validate(It.IsAny<CreditCard>()));
        }

        [Fact]
        public async Task UpdateCreditCardAccountAsync_Uses_Correct_Endpoint()
        {
            // Arrange
            var cc = new CreditCard();
            Uri endpoint = null;

            webServiceRequest.Setup(m => m.PutDeserializedAsync<CreditCard, Result<CreditCard>>(It.IsAny<Uri>(), It.IsAny<CreditCard>()))
                .Callback((Uri a, CreditCard b) => endpoint = a)
                .ReturnsAsync(new Result<CreditCard>());

            // Act
            await service.UpdateCreditCardAccountAsync(cc);

            // Assert
            Assert.Equal(string.Format("{0}/v4/account/creditcard", settings.BaseUrl), endpoint.AbsoluteUri);
        }

        [Fact]
        public async Task UpdateCreditCardAccountAsync_Verify_WebServiceRequest_PutDeserializedAsync()
        {
            // Arrange
            webServiceRequest.Setup(m => m.PutDeserializedAsync<CreditCard, Result<CreditCard>>(It.IsAny<Uri>(), It.IsAny<CreditCard>())).ReturnsAsync(new Result<CreditCard> { Response = new CreditCard() });
            var cc = new CreditCard();

            // Act
            await service.UpdateCreditCardAccountAsync(cc);

            // Assert
            webServiceRequest.Verify(m => m.PutDeserializedAsync<CreditCard, Result<CreditCard>>(It.IsAny<Uri>(), It.IsAny<CreditCard>()));
        }
    }
}