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
    public class AccountTests
    {
        [Test]
        [Category("Unit")]
        public void GetBalanceInformationTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"balance\":20.5,\r\n   \"currency\":{\r\n      \"code\":\"EUR\",\r\n      \"name\":\"Euro\",\r\n      \"sign\":\"€\"\r\n   }\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<BalanceInfo> balanceInfo = manager.GetBalanceInformation();

            Assert.NotNull(balanceInfo);
            Assert.True(balanceInfo.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, balanceInfo.StatusCode);

            Assert.AreEqual(20.5m, balanceInfo.Response.balance);
            Assert.AreEqual("Euro", balanceInfo.Response.currency.name);
            Assert.AreEqual("EUR", balanceInfo.Response.currency.code);
            Assert.AreEqual("€", balanceInfo.Response.currency.sign);

        }

        [Test]
        [Category("Unit")]
        public void GetPricesOfServicesTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n\"sms\":[\r\n      {\r\n         \"mcc\":\"202\",\r\n         \"country\":\"Greece\",\r\n         \"iso\":\"GR\",\r\n         \"networks\":[\r\n            {\r\n               \"network\":\"Cosmote\",\r\n               \"mnc\":\"01\",\r\n               \"price\":0.042\r\n            },\r\n            {\r\n               \"network\":\"Wind\",\r\n               \"mnc\":\"09\",\r\n               \"price\":0.042\r\n            }\r\n         ]\r\n      },\r\n      {\r\n         \"mcc\":\"204\",\r\n         \"country\":\"Netherlands\",\r\n         \"iso\":\"NL\",\r\n         \"networks\":[\r\n            {\r\n               \"network\":\"Tele2\",\r\n               \"mnc\":\"02\",\r\n               \"price\":0.042\r\n            },\r\n            {\r\n               \"network\":\"Vodafone\",\r\n               \"mnc\":\"04\",\r\n               \"price\":0.042\r\n            }\r\n         ]\r\n      }\r\n   ],\r\n   \"lookup\":{\r\n      \"PerRequest\":\"0.009\"\r\n   },\r\n   \"twoStep\":{\r\n      \"SmsVerification\":\"0.06\",\r\n      \"VoiceVerification\":\"0.07\"\r\n   },\r\n   \"currency\":{\r\n      \"code\":\"EUR\",\r\n      \"name\":\"Euro\",\r\n      \"sign\":\"€\"\r\n   }\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<RouteeServicesPrice> prices = manager.GetPricesOfAllServices();

            Assert.NotNull(prices);
            Assert.True(prices.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, prices.StatusCode);

            Assert.AreEqual("Greece", prices.Response.sms[0].country);
            Assert.AreEqual("GR", prices.Response.sms[0].iso);
            Assert.AreEqual("202", prices.Response.sms[0].mcc);
            Assert.AreEqual("Cosmote", prices.Response.sms[0].networks[0].network);
            Assert.AreEqual("01", prices.Response.sms[0].networks[0].mnc);
            Assert.AreEqual(0.042m, prices.Response.sms[0].networks[0].price);

            Assert.AreEqual(0.009m, prices.Response.lookup.PerRequest);

            Assert.AreEqual(0.06m, prices.Response.twoStep.SmsVerification);
            Assert.AreEqual(0.07m, prices.Response.twoStep.VoiceVerification);

            Assert.AreEqual("Euro", prices.Response.currency.name);
            Assert.AreEqual("EUR", prices.Response.currency.code);
            Assert.AreEqual("€", prices.Response.currency.sign);

        }

        [Test]
        [Category("Unit")]
        public void GetAccountTransactionTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"totalPages\":5,\r\n   \"totalElements\":100,\r\n   \"last\":false,\r\n   \"numberOfElements\":1,\r\n   \"first\":true,\r\n   \"size\":20,\r\n   \"number\":0,\r\n   \"content\":[\r\n      {\r\n         \"id\":\"asdasd\",\r\n         \"source\":\"Paypal\",\r\n         \"transactionType\":\"TopUp\",\r\n         \"amount\":\"100.0\",\r\n         \"status\":\"Pending\",\r\n         \"balanceBefore\":\"0.00\",\r\n         \"balanceAfter\":\"100\",\r\n         \"date\":\"2016-01-01T17:00:00.000Z\",\r\n         \"actions\":[\r\n            {\r\n               \"id\":\"32\",\r\n               \"type\":\"Paid\",\r\n               \"amount\":\"50.0\",\r\n               \"date\":\"2016-01-02T18:00:00.000Z\",\r\n               \"balanceBefore\":\"100\",\r\n               \"balanceAfter\":\"100\",\r\n               \"status\":\"Completed\"\r\n            },\r\n            {\r\n               \"id\":\"54\",\r\n               \"type\":\"Credit\",\r\n               \"amount\":\"50.0\",\r\n               \"date\":\"2016-01-03T19:00:00.000Z\",\r\n               \"balanceBefore\":\"100\",\r\n               \"balanceAfter\":\"150\",\r\n               \"status\":\"Completed\"\r\n            }\r\n         ]\r\n      }\r\n   ]\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<TransactionsHistory> transactionHistory = manager.GetAccountTransactions();

            Assert.NotNull(transactionHistory);
            Assert.True(transactionHistory.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, transactionHistory.StatusCode);

            Assert.AreEqual(5m, transactionHistory.Response.totalPages);
            Assert.AreEqual(100m, transactionHistory.Response.totalElements);
            Assert.AreEqual(1m, transactionHistory.Response.numberOfElements);
            Assert.AreEqual(20m, transactionHistory.Response.size);
            Assert.AreEqual(0m, transactionHistory.Response.number);
            Assert.False(transactionHistory.Response.last);
            Assert.True(transactionHistory.Response.first);

            Assert.AreEqual("asdasd", transactionHistory.Response.content[0].id);
            Assert.AreEqual("Paypal", transactionHistory.Response.content[0].source);
            Assert.AreEqual("TopUp", transactionHistory.Response.content[0].transactionType);
            Assert.AreEqual(100m, transactionHistory.Response.content[0].amount);
            Assert.AreEqual("Pending", transactionHistory.Response.content[0].status);
            Assert.AreEqual(0m, transactionHistory.Response.content[0].balanceBefore);
            Assert.AreEqual(100m, transactionHistory.Response.content[0].balanceAfter);

            Assert.AreEqual("32", transactionHistory.Response.content[0].actions[0].id);
            Assert.AreEqual("Paid", transactionHistory.Response.content[0].actions[0].type);
            Assert.AreEqual(50m, transactionHistory.Response.content[0].actions[0].amount);
            Assert.AreEqual(100m, transactionHistory.Response.content[0].actions[0].balanceBefore);
            Assert.AreEqual(100m, transactionHistory.Response.content[0].actions[0].balanceAfter);
            Assert.AreEqual("Completed", transactionHistory.Response.content[0].actions[0].status);





        }

        [Test]
        [Category("Unit")]
        public void GetAvailableBankAccountsTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"name\":\"AMD\",\r\n   \"address\":\"our address\",\r\n   \"phone\":\"our phone\",\r\n   \"vatId\":\"our vat id\",\r\n   \"email\":\"email@routee.net\",\r\n   \"banks\":[  \r\n      {  \r\n         \"name\":\"A bank\",\r\n         \"address\":\"bank\'s address\",\r\n         \"number\":\"our bank account number\",\r\n         \"iban\":\"our iban\",\r\n         \"currency\":\"the currency of our bank account\",\r\n         \"minAmount\":\"10\",\r\n         \"country\":\"bank\'s country\",\r\n         \"swiftCode\":\"bank\'s swift code\"\r\n      }\r\n   ]\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<BankAccount> backAccounts = manager.GetAvailableBankAccounts();

            Assert.NotNull(backAccounts);
            Assert.True(backAccounts.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, backAccounts.StatusCode);

            Assert.AreEqual("AMD", backAccounts.Response.name);
            Assert.AreEqual("our address", backAccounts.Response.address);
            Assert.AreEqual("our phone", backAccounts.Response.phone);
            Assert.AreEqual("our vat id", backAccounts.Response.vatId);
            Assert.AreEqual("email@routee.net", backAccounts.Response.email);

            Assert.AreEqual("A bank", backAccounts.Response.banks[0].name);
            Assert.AreEqual("bank's address", backAccounts.Response.banks[0].address);
            Assert.AreEqual("our bank account number", backAccounts.Response.banks[0].number);
            Assert.AreEqual("our iban", backAccounts.Response.banks[0].iban);
            Assert.AreEqual("the currency of our bank account", backAccounts.Response.banks[0].currency);
            Assert.AreEqual(10m, backAccounts.Response.banks[0].minAmount);
            Assert.AreEqual("bank's country", backAccounts.Response.banks[0].country);
            Assert.AreEqual("bank's swift code", backAccounts.Response.banks[0].swiftCode);

        }
    }
}
