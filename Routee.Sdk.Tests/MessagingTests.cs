using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Moq;
using NUnit.Framework;
using RestSharp;
using Routee.Sdk.Models;

namespace Routee.Sdk.Tests
{
    public class MessagingTests
    {
        [Test]
        [Category("Unit")]
        public void SendSingleSmsTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"trackingId\": \"2c1379bb-f296-43a4-bfb0-6c8b20a97425\",\r\n   \"status\": \"Queued\",\r\n   \"createdAt\": \"2015-12-17T11:57:31.293Z\",\r\n   \"from\": \"amdTelecom\",\r\n   \"to\": \"+306973359355\",\r\n   \"body\": \"A new game has been posted to the MindPuzzle. Check it out\",\r\n   \"bodyAnalysis\":       {\r\n      \"parts\": 1,\r\n      \"unicode\": false,\r\n      \"characters\": 58\r\n   },\r\n   \"flash\": false,\r\n   \"callback\":    {\r\n      \"url\": \"http://www.yourserver.com/message\",\r\n      \"strategy\": \"OnChange\"\r\n   }\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            SingleSms sms=new SingleSms();
            sms.body = "A new game has been posted to the MindPuzzle. Check it out";
            sms.to = "+306973359355";
            sms.from = "amdTelecom";

            sms.callback=new Callback();
            sms.callback.strategy = CallbackStrategy.OnChange;
            sms.callback.url = "http://www.yourserver.com/message";
            
            RouteResponse<SmsResponse> smsResponse = manager.SendSms(sms);

            Assert.NotNull(smsResponse);
            Assert.True(smsResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, smsResponse.StatusCode);

            Assert.AreEqual("2c1379bb-f296-43a4-bfb0-6c8b20a97425", smsResponse.Response.trackingId);
            Assert.AreEqual("Queued", smsResponse.Response.status);
            Assert.AreEqual("+306973359355", smsResponse.Response.to);
            Assert.AreEqual("amdTelecom", smsResponse.Response.from);
            Assert.AreEqual("A new game has been posted to the MindPuzzle. Check it out", smsResponse.Response.body);

            Assert.False(smsResponse.Response.flash);
            Assert.AreEqual(CallbackStrategy.OnChange, smsResponse.Response.callback.strategy);
            Assert.AreEqual("http://www.yourserver.com/message", smsResponse.Response.callback.url);

            Assert.AreEqual(1m, smsResponse.Response.bodyAnalysis.parts);
            Assert.AreEqual(false, smsResponse.Response.bodyAnalysis.unicode);
            Assert.AreEqual(58m, smsResponse.Response.bodyAnalysis.characters);


        }

        [Test]
        [Category("Unit")]
        public void AnalyzeSingleSmsTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"cost\":0.042,\r\n   \"bodyAnalysis\":{\r\n      \"parts\":1,\r\n      \"unicode\":false,\r\n      \"characters\":58\r\n   }\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            BaseSms sms = new BaseSms();
            sms.body = "A new game has been posted to the MindPuzzle. Check it out";
            sms.to = "+306973359355";
            sms.from = "amdTelecom";

            RouteResponse<AnalyzeMessageResponse> smsResponse = manager.AnalyzeSingleSms(sms);

            Assert.NotNull(smsResponse);
            Assert.True(smsResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, smsResponse.StatusCode);

            Assert.AreEqual(0.042m, smsResponse.Response.cost);
            Assert.AreEqual(1m, smsResponse.Response.bodyAnalysis.parts);
            Assert.AreEqual(false, smsResponse.Response.bodyAnalysis.unicode);
            Assert.AreEqual(58m, smsResponse.Response.bodyAnalysis.characters);


        }

        [Test]
        [Category("Unit")]
        public void SendCampaign()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"trackingId\":\"182d20da-1cd1-4d94-b3f5-c2dd38352d22\",\r\n   \"type\":\"Sms\",\r\n   \"state\":\"Queued\",\r\n   \"createdAt\":\"2015-12-17T15:01:42.395Z\",\r\n   \"from\":\"mindpuzzle\",\r\n   \"groups\":[\r\n      \"customers\"\r\n   ],\r\n   \"body\":\"Hello [~firstName] a new version of MindPuzzle is available. Check it out\",\r\n   \"smsAnalysis\":{\r\n      \"numberOfRecipients\":30,\r\n      \"recipientsPerCountry\":{\r\n         \"GR\":25,\r\n         \"US\":5\r\n      },\r\n      \"recipientCountries\":{\r\n\r\n      },\r\n      \"contacts\":{\r\n\r\n      },\r\n      \"recipientsPerGroup\":{\r\n         \"customers\":30\r\n      },\r\n      \"totalInGroups\":30,\r\n      \"bodyAnalysis\":{\r\n         \"parts\":1,\r\n         \"unicode\":false,\r\n         \"characters\":74\r\n      }\r\n   },\r\n   \"flash\":false,\r\n   \"respectQuietHours\":false,\r\n   \"statuses\":{\r\n      \"Queued\":0,\r\n      \"Sent\":0,\r\n      \"Unsent\":0,\r\n      \"Failed\":0,\r\n      \"Delivered\":0,\r\n      \"Undelivered\":0\r\n   },\r\n   \"campaignCallback\":{\r\n      \"strategy\":\"OnCompletion\",\r\n      \"url\":\"http://www.yourserver.com/campaign\"\r\n   },\r\n   \"callback\":{\r\n      \"strategy\":\"OnChange\",\r\n      \"url\":\"http://www.yourserver.com/message\"\r\n   }\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            Campaign campaign = new Campaign();
            campaign.body = "Hello [~firstName] a new version of MindPuzzle is available. Check it out";
            campaign.groups = (new[] {"customers"}).ToList();
            campaign.from = "mindpuzzle";
            campaign.campaignCallback=new CampaignCallback();
            campaign.campaignCallback.strategy = CallbackStrategy.OnCompletion;
            campaign.campaignCallback.url = "http://www.yourserver.com/campaign";

            campaign.callback = new Callback();
            campaign.callback.strategy = CallbackStrategy.OnChange;
            campaign.callback.url = "http://www.yourserver.com/message";


            RouteResponse<CampaignResponse> campaignResponse = manager.SendSmsCampaign(campaign);

            Assert.NotNull(campaignResponse);
            Assert.True(campaignResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, campaignResponse.StatusCode);

            Assert.AreEqual("182d20da-1cd1-4d94-b3f5-c2dd38352d22", campaignResponse.Response.trackingId);
            Assert.AreEqual("Sms", campaignResponse.Response.type);
            Assert.AreEqual("Queued", campaignResponse.Response.state);
            Assert.AreEqual("mindpuzzle", campaignResponse.Response.from);
            Assert.AreEqual("customers", campaignResponse.Response.groups[0]);
            Assert.AreEqual("Hello [~firstName] a new version of MindPuzzle is available. Check it out", campaignResponse.Response.body);
            Assert.AreEqual(CallbackStrategy.OnCompletion, campaignResponse.Response.campaignCallback.strategy);
            Assert.AreEqual("http://www.yourserver.com/campaign", campaignResponse.Response.campaignCallback.url);
            Assert.AreEqual(CallbackStrategy.OnChange, campaignResponse.Response.callback.strategy);
            Assert.AreEqual("http://www.yourserver.com/message", campaignResponse.Response.callback.url);
            Assert.AreEqual(false, campaignResponse.Response.respectQuietHours);
            Assert.AreEqual(false, campaignResponse.Response.flash);
            Assert.AreEqual(6, campaignResponse.Response.statuses.Count);
            Assert.AreEqual(30m, campaignResponse.Response.smsAnalysis.numberOfRecipients);
            Assert.AreEqual(2, campaignResponse.Response.smsAnalysis.recipientsPerCountry.Count);
            Assert.AreEqual(1, campaignResponse.Response.smsAnalysis.recipientsPerGroup.Count);
            Assert.AreEqual(30m, campaignResponse.Response.smsAnalysis.totalInGroups);
            Assert.AreEqual(1m, campaignResponse.Response.smsAnalysis.bodyAnalysis.parts);
            Assert.AreEqual(false, campaignResponse.Response.smsAnalysis.bodyAnalysis.unicode);
            Assert.AreEqual(74m, campaignResponse.Response.smsAnalysis.bodyAnalysis.characters);

        }

        [Test]
        [Category("Unit")]
        public void AnalyzeACampaign()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"numberOfRecipients\":30,\r\n   \"recipientsPerCountry\":{\r\n      \"GR\":25,\r\n      \"US\":5\r\n   },\r\n   \"recipientCountries\":{\r\n\r\n   },\r\n   \"contacts\":{\r\n\r\n   },\r\n   \"recipientsPerGroup\":{\r\n      \"customers\":30\r\n   },\r\n   \"totalInGroups\":30,\r\n   \"bodyAnalysis\":{\r\n      \"parts\":1,\r\n      \"unicode\":false,\r\n      \"characters\":74\r\n   }\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            AnalyzeCampaign campaign = new AnalyzeCampaign();
            campaign.body = "Hello [~firstName] a new version of MindPuzzle is available. Check it out";
            campaign.groups = (new[] { "customers" }).ToList();
            campaign.from = "mindpuzzle";

            RouteResponse<AnalyzeCampaignResponse> campaignResponse = manager.AnalyzeCampaign(campaign);

            Assert.NotNull(campaignResponse);
            Assert.True(campaignResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, campaignResponse.StatusCode);
            Assert.AreEqual(30m, campaignResponse.Response.numberOfRecipients);
            Assert.AreEqual(2, campaignResponse.Response.recipientsPerCountry.Count);
            Assert.AreEqual(25m, campaignResponse.Response.recipientsPerCountry["GR"]);
            Assert.AreEqual(5m, campaignResponse.Response.recipientsPerCountry["US"]);
            Assert.AreEqual(1, campaignResponse.Response.recipientsPerGroup.Count);
            Assert.AreEqual(30m, campaignResponse.Response.recipientsPerGroup["customers"]);
            Assert.AreEqual(30m, campaignResponse.Response.totalInGroups);
            Assert.AreEqual(1m, campaignResponse.Response.bodyAnalysis.parts);
            Assert.AreEqual(false, campaignResponse.Response.bodyAnalysis.unicode);
            Assert.AreEqual(74m, campaignResponse.Response.bodyAnalysis.characters);

        }

        [Test]
        [Category("Unit")]
        public void TrackSingleMessageTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "[\r\n      {\r\n         \"smsId\":\"b6ba5647-1ebd-49ab-9fd3-5a9cebb26ff2\",\r\n         \"messageId\":\"a6b62347-5e7d-49ab-9fd3-5a9cebb26hj5\",\r\n         \"to\":\"9647912345628\",\r\n         \"groups\":[\r\n\r\n         ],\r\n         \"country\":\"Iraq\",\r\n         \"operator\":\"Test_Operator\",\r\n         \"status\":{\r\n            \"status\":\"Delivered\",\r\n            \"date\":\"2015-11-11T17:07:04Z\"\r\n         },\r\n         \"body\":\"Hello how are u?\",\r\n      }\r\n]"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<List<TrackSmsResponse>> smsResponse = manager.TrackSingleMessage("b6ba5647-1ebd-49ab-9fd3-5a9cebb26ff2");

            Assert.NotNull(smsResponse);
            Assert.True(smsResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, smsResponse.StatusCode);

            Assert.AreEqual("b6ba5647-1ebd-49ab-9fd3-5a9cebb26ff2", smsResponse.Response[0].smsId);
            Assert.AreEqual("a6b62347-5e7d-49ab-9fd3-5a9cebb26hj5", smsResponse.Response[0].messageId);
            Assert.AreEqual("9647912345628", smsResponse.Response[0].to);
            Assert.AreEqual("Iraq", smsResponse.Response[0].country);
            Assert.AreEqual("Test_Operator", smsResponse.Response[0].@operator);
            Assert.AreEqual("Hello how are u?", smsResponse.Response[0].body);
            Assert.AreEqual("Delivered", smsResponse.Response[0].status.status);
        }

        [Test]
        [Category("Unit")]
        public void TrackMessagesOfACampaignTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"content\":[\r\n      {\r\n         \"messageId\":\"b6ba5647-1ebd-49ab-9fd3-5a9cebb26ff2\",\r\n         \"to\":\"9647912345628\",\r\n         \"groups\":[\r\n\r\n         ],\r\n         \"country\":\"Iraq\",\r\n         \"operator\":\"Test_Operator\",\r\n         \"status\":{\r\n            \"status\":\"Delivered\",\r\n            \"date\":\"2015-11-11T17:07:04Z\"\r\n         },\r\n         \"body\":\"Hello how are u?\",\r\n         \"campaign\":\"SmsCampaign\"\r\n      }\r\n   ],\r\n   \"totalPages\":1,\r\n   \"last\":true,\r\n   \"totalElements\":1,\r\n   \"first\":true,\r\n   \"numberOfElements\":1,\r\n   \"size\":20,\r\n   \"number\":0\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<TrackSmsCampaignResponse> campaignResponse = manager.TrackMessagesOfACampaign("b6ba5647-1ebd-49ab-9fd3-5a9cebb26ff2");

            Assert.NotNull(campaignResponse);
            Assert.True(campaignResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, campaignResponse.StatusCode);

            Assert.AreEqual(1m, campaignResponse.Response.totalPages);
            Assert.AreEqual(1m, campaignResponse.Response.totalElements);
            Assert.AreEqual(1m, campaignResponse.Response.numberOfElements);
            Assert.AreEqual(20m, campaignResponse.Response.size);
            Assert.AreEqual(0m, campaignResponse.Response.number);
            Assert.True(campaignResponse.Response.last);
            Assert.True(campaignResponse.Response.first);

            
            Assert.AreEqual("b6ba5647-1ebd-49ab-9fd3-5a9cebb26ff2", campaignResponse.Response.content[0].messageId);
            Assert.AreEqual("9647912345628", campaignResponse.Response.content[0].to);
            Assert.AreEqual("Iraq", campaignResponse.Response.content[0].country);
            Assert.AreEqual("Test_Operator", campaignResponse.Response.content[0].@operator);
            Assert.AreEqual("Hello how are u?", campaignResponse.Response.content[0].body);
            Assert.AreEqual("Delivered", campaignResponse.Response.content[0].status.status);
        }

        [Test]
        [Category("Unit")]
        public void TrackMessagesWithFilters()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"content\":[\r\n      {\r\n         \"messageId\":\"b6ba5647-1ebd-49ab-9fd3-5a9cebb26ff2\",\r\n         \"to\":\"9647912345628\",\r\n         \"groups\":[\r\n\r\n         ],\r\n         \"country\":\"Iraq\",\r\n         \"operator\":\"Test_Operator\",\r\n         \"status\":{\r\n            \"status\":\"Delivered\",\r\n            \"date\":\"2015-11-11T17:07:04Z\"\r\n         },\r\n         \"body\":\"Hello how are u?\",\r\n         \"campaign\":\"Single Sms Campaign With Callback5\"\r\n      }\r\n   ],\r\n   \"totalPages\":1,\r\n   \"last\":true,\r\n   \"totalElements\":1,\r\n   \"first\":true,\r\n   \"numberOfElements\":1,\r\n   \"size\":20,\r\n   \"number\":0\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            TrackMultipleSms multipleSms=new TrackMultipleSms();
            multipleSms.fieldName = "id";
            multipleSms.searchTerm = "a9122812-21e0-47c1-ba13-746e4a212655";
            List<TrackMultipleSms> list=new List<TrackMultipleSms>();
            list.Add(multipleSms);
            RouteResponse<TrackSmsCampaignResponse> trackSmsResponse = manager.TrackMultipleSmsWithFilters(list);

            Assert.NotNull(trackSmsResponse);
            Assert.True(trackSmsResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, trackSmsResponse.StatusCode);

            Assert.AreEqual(1m, trackSmsResponse.Response.totalPages);
            Assert.AreEqual(1m, trackSmsResponse.Response.totalElements);
            Assert.AreEqual(1m, trackSmsResponse.Response.numberOfElements);
            Assert.AreEqual(20m, trackSmsResponse.Response.size);
            Assert.AreEqual(0m, trackSmsResponse.Response.number);
            Assert.True(trackSmsResponse.Response.last);
            Assert.True(trackSmsResponse.Response.first);


            Assert.AreEqual("b6ba5647-1ebd-49ab-9fd3-5a9cebb26ff2", trackSmsResponse.Response.content[0].messageId);
            Assert.AreEqual("9647912345628", trackSmsResponse.Response.content[0].to);
            Assert.AreEqual("Iraq", trackSmsResponse.Response.content[0].country);
            Assert.AreEqual("Test_Operator", trackSmsResponse.Response.content[0].@operator);
            Assert.AreEqual("Hello how are u?", trackSmsResponse.Response.content[0].body);
            Assert.AreEqual("Delivered", trackSmsResponse.Response.content[0].status.status);
        }

        [Test]
        [Category("Unit")]
        public void RetrieveQueitHourCountryListTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "[\r\n   {\r\n      \"code\":\"GR\",\r\n      \"name\":\"Greece\",\r\n      \"localeName\":\"Ελλάδα\",\r\n      \"supported\":true\r\n   },\r\n   {\r\n      \"code\":\"US\",\r\n      \"name\":\"United States of America\",\r\n      \"localeName\":\"United States of America\",\r\n      \"supported\":false\r\n   }\r\n]"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<List<QuietHourCountry>> quiteHourList = manager.RetrieveQueitHourCountryList("en");

            Assert.NotNull(quiteHourList);
            Assert.True(quiteHourList.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, quiteHourList.StatusCode);

            Assert.AreEqual("Greece", quiteHourList.Response[0].name);
            Assert.AreEqual("GR", quiteHourList.Response[0].code);
            Assert.AreEqual(true, quiteHourList.Response[0].supported);
            Assert.AreEqual("Ελλάδα", quiteHourList.Response[0].localeName);
        }

        [Test]
        [Category("Unit")]
        public void UpdateScheduledMessageCampaign()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"trackingId\":\"182d20da-1cd1-4d94-b3f5-c2dd38352d22\",\r\n   \"type\":\"Sms\",\r\n   \"state\":\"Queued\",\r\n   \"createdAt\":\"2015-12-17T15:01:42.395Z\",\r\n   \"from\":\"amdTelecom2\",\r\n   \"to\":[\r\n      \"+306984512344\"\r\n   ],\r\n   \"body\":\"hellooo my friendd2!!!\",\r\n   \"smsAnalysis\":{\r\n      \"numberOfRecipients\":1,\r\n      \"recipientsPerCountry\":{\r\n         \"GR\":1\r\n      },\r\n      \"recipientCountries\":{\r\n         \"+306984512344\":\"GR\"\r\n      },\r\n      \"contacts\":{\r\n\r\n      },\r\n      \"recipientsPerGroup\":{\r\n\r\n      },\r\n      \"totalInGroups\":0,\r\n      \"bodyAnalysis\":{\r\n         \"parts\":1,\r\n         \"unicode\":false,\r\n         \"characters\":22\r\n      }\r\n   },\r\n   \"flash\":false,\r\n   \"respectQuietHours\":false,\r\n   \"statuses\":{\r\n      \"Queued\":0,\r\n      \"Sent\":0,\r\n      \"Failed\":0,\r\n      \"Delivered\":0,\r\n      \"Undelivered\":0\r\n   }\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            Campaign campaign = new Campaign();
            campaign.body = "hellooo my friendd2!!!";
            campaign.to = (new[] { "+306984512344" }).ToList();
            campaign.from = "amdTelecom2";

            RouteResponse<CampaignResponse> campaignResponse = manager.UpdateSmsCampaign("182d20da-1cd1-4d94-b3f5-c2dd38352d22", campaign);

            Assert.NotNull(campaignResponse);
            Assert.True(campaignResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, campaignResponse.StatusCode);

            Assert.AreEqual("182d20da-1cd1-4d94-b3f5-c2dd38352d22", campaignResponse.Response.trackingId);
            Assert.AreEqual("Sms", campaignResponse.Response.type);
            Assert.AreEqual("Queued", campaignResponse.Response.state);
            Assert.AreEqual("amdTelecom2", campaignResponse.Response.from);
            Assert.AreEqual("+306984512344", campaignResponse.Response.to[0]);
            Assert.AreEqual("hellooo my friendd2!!!", campaignResponse.Response.body);
            Assert.AreEqual(false, campaignResponse.Response.respectQuietHours);
            Assert.AreEqual(false, campaignResponse.Response.flash);
            Assert.AreEqual(5, campaignResponse.Response.statuses.Count);
            Assert.AreEqual(1m, campaignResponse.Response.smsAnalysis.numberOfRecipients);
            Assert.AreEqual(1, campaignResponse.Response.smsAnalysis.recipientsPerCountry.Count);
            Assert.AreEqual(1, campaignResponse.Response.smsAnalysis.recipientCountries.Count);
            Assert.AreEqual("GR", campaignResponse.Response.smsAnalysis.recipientCountries["+306984512344"]);
            Assert.AreEqual(0m, campaignResponse.Response.smsAnalysis.totalInGroups);
            Assert.AreEqual(1m, campaignResponse.Response.smsAnalysis.bodyAnalysis.parts);
            Assert.AreEqual(false, campaignResponse.Response.smsAnalysis.bodyAnalysis.unicode);
            Assert.AreEqual(22m, campaignResponse.Response.smsAnalysis.bodyAnalysis.characters);
        }

        [Test]
        [Category("Unit")]
        public void DeleteCampaign()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<object> campaignResponse = manager.DeleteScheduledCampaign("182d20da-1cd1-4d94-b3f5-c2dd38352d22");

            Assert.NotNull(campaignResponse);
            Assert.True(campaignResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, campaignResponse.StatusCode);
        }

        [Test]
        [Category("Unit")]
        public void RetrievedetailsOfACampaign()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"trackingId\":\"182d20da-1cd1-4d94-b3f5-c2dd38352d22\",\r\n   \"type\":\"Sms\",\r\n   \"state\":\"Queued\",\r\n   \"createdAt\":\"2015-12-17T15:01:42.395Z\",\r\n   \"from\":\"mindpuzzle\",\r\n   \"groups\":[\r\n      \"customers\"\r\n   ],\r\n   \"body\":\"Hello [~firstName] a new version of MindPuzzle is available. Check it out\",\r\n   \"smsAnalysis\":{\r\n      \"numberOfRecipients\":30,\r\n      \"recipientsPerCountry\":{\r\n         \"GR\":25,\r\n         \"US\":5\r\n      },\r\n      \"recipientCountries\":{\r\n\r\n      },\r\n      \"contacts\":{\r\n\r\n      },\r\n      \"recipientsPerGroup\":{\r\n         \"customers\":30\r\n      },\r\n      \"totalInGroups\":30,\r\n      \"bodyAnalysis\":{\r\n         \"parts\":1,\r\n         \"unicode\":false,\r\n         \"characters\":74\r\n      }\r\n   },\r\n   \"flash\":false,\r\n   \"respectQuietHours\":true,\r\n   \"statuses\":{\r\n      \"Queued\":0,\r\n      \"Sent\":25,\r\n      \"Unsent\":25,\r\n      \"Failed\":5,\r\n      \"Delivered\":0,\r\n      \"Undelivered\":0\r\n   },\r\n   \"campaignCallback\":{\r\n      \"strategy\":\"OnCompletion\",\r\n      \"url\":\"http://www.yourserver.com/campaign\"\r\n   },\r\n   \"callback\":{\r\n      \"strategy\":\"OnChange\",\r\n      \"url\":\"http://www.yourserver.com/message\"\r\n   }, \r\n   \"quietHoursReport\":{\r\n      \"GR\": \"25\"\r\n   }\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<CampaignResponse> campaignResponse = manager.GetCampaignDetails("182d20da-1cd1-4d94-b3f5-c2dd38352d22");

            Assert.NotNull(campaignResponse);
            Assert.True(campaignResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, campaignResponse.StatusCode);

            Assert.AreEqual("182d20da-1cd1-4d94-b3f5-c2dd38352d22", campaignResponse.Response.trackingId);
            Assert.AreEqual("Sms", campaignResponse.Response.type);
            Assert.AreEqual("Queued", campaignResponse.Response.state);
            Assert.AreEqual("mindpuzzle", campaignResponse.Response.from);
            Assert.AreEqual("customers", campaignResponse.Response.groups[0]);
            Assert.AreEqual("Hello [~firstName] a new version of MindPuzzle is available. Check it out", campaignResponse.Response.body);
            Assert.AreEqual(CallbackStrategy.OnCompletion, campaignResponse.Response.campaignCallback.strategy);
            Assert.AreEqual("http://www.yourserver.com/campaign", campaignResponse.Response.campaignCallback.url);
            Assert.AreEqual(CallbackStrategy.OnChange, campaignResponse.Response.callback.strategy);
            Assert.AreEqual("http://www.yourserver.com/message", campaignResponse.Response.callback.url);
            Assert.AreEqual(true, campaignResponse.Response.respectQuietHours);
            Assert.AreEqual(false, campaignResponse.Response.flash);
            Assert.AreEqual(6, campaignResponse.Response.statuses.Count);
            Assert.AreEqual(30m, campaignResponse.Response.smsAnalysis.numberOfRecipients);
            Assert.AreEqual(2, campaignResponse.Response.smsAnalysis.recipientsPerCountry.Count);
            Assert.AreEqual(1, campaignResponse.Response.smsAnalysis.recipientsPerGroup.Count);
            Assert.AreEqual(30m, campaignResponse.Response.smsAnalysis.totalInGroups);
            Assert.AreEqual(1m, campaignResponse.Response.smsAnalysis.bodyAnalysis.parts);
            Assert.AreEqual(false, campaignResponse.Response.smsAnalysis.bodyAnalysis.unicode);
            Assert.AreEqual(74m, campaignResponse.Response.smsAnalysis.bodyAnalysis.characters);
            Assert.AreEqual(1m, campaignResponse.Response.quietHoursReport.Count);
            Assert.AreEqual(25, campaignResponse.Response.quietHoursReport["GR"]);

        }
    }
}
