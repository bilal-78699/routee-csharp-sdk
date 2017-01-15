using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RestSharp;
using Routee.Sdk.Models;

namespace Routee.Sdk.Tests
{
    [TestFixture]
    public class LoginTests
    {
        [Test]
        [Category("Unit")]
        public void LoginTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"access_token\":\"346f0dd4-c7bb-4f84-924e-47459c7d11c6\",\r\n   \"token_type\":\"bearer\",\r\n   \"expires_in\":1999,\r\n   \"scope\":\"account contact report sms\",\r\n   \"permissions\":[\r\n      \"MT_PERMISSION_TRANSLATION_READ\"\r\n   ]\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<LoginResponse> loginResponse = manager.Login();

            Assert.NotNull(loginResponse);
            Assert.True(loginResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

            Assert.AreEqual("1999", loginResponse.Response.expires_in);
            Assert.AreEqual("346f0dd4-c7bb-4f84-924e-47459c7d11c6", loginResponse.Response.access_token);
            Assert.AreEqual("account contact report sms", loginResponse.Response.scope);
            Assert.AreEqual("MT_PERMISSION_TRANSLATION_READ", loginResponse.Response.permissions[0]);
            
        }
    }
}
