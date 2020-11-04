using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VacancyInfo;
using VacancyInfo.Models;
using VacancyInfo.Services;

namespace VacancyInfoTests.ServicesTests
{
    [TestClass]
    public class VacanRequestServiceTestscyServiceTests
    {
        private string goodRequest = "https://api.hh.ru/vacancies?text=велосипедист";

        public VacanRequestServiceTestscyServiceTests()
        {

        }

        [TestMethod]
        public void SendRequest_WrontUrl_ReturnError()
        {
            // Arrange
            var httmMock = GetMockClientFactory(HttpStatusCode.OK);
            RequestService requestServices = new RequestService(httmMock.Object);

            // Assert
            try
            {
                var requestRes = requestServices.SendRequest("asd").GetAwaiter().GetResult();
                Assert.Fail();
            }
            catch (Exception e)
            {                
                // Act
                Assert.IsTrue(e.Message == "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.");
            }
            
        }

        [TestMethod]
        public void SendRequest_GoodUrl_ReturnAnyStream()
        {
            // Arrange
            var httmMock = GetMockClientFactory(HttpStatusCode.OK);
            RequestService requestServices = new RequestService(httmMock.Object);

            // Assert
            var requestRes = requestServices.SendRequest(goodRequest).GetAwaiter().GetResult();
            // Act
            Assert.IsTrue(requestRes.Length > 0);
        }

        [TestMethod]
        public void SendRequest_WithHttpOKStatus_ReturnAnyStream()
        {
            // Arrange
            var httmMock = GetMockClientFactory(HttpStatusCode.OK);
            RequestService requestServices = new RequestService(httmMock.Object);

            // Assert
            var requestRes = requestServices.SendRequest(goodRequest).GetAwaiter().GetResult();

            // Act
            Assert.IsTrue(requestRes.Length > 0);
        }

        [TestMethod]
        public void SendRequest_WithHttpBadRequestStatus_ReturnNullStream()
        {
            // Arrange
            var httmMock = GetMockClientFactory(HttpStatusCode.BadRequest);
            RequestService requestServices = new RequestService(httmMock.Object);

            // Assert
            var requestRes = requestServices.SendRequest(goodRequest).GetAwaiter().GetResult();

            // Act
            Assert.IsTrue(requestRes == Stream.Null);
        }

        private Mock<IHttpClientFactory> GetMockClientFactory(HttpStatusCode statusCode)
        {
            var httmMock = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent("{'name':thecodebuzz,'city':'USA'}"),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            httmMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            return httmMock;
        }
    }
}
