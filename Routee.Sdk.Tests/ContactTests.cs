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
using Routee.Sdk.Catalogs;
using Routee.Sdk.Models;

namespace Routee.Sdk.Tests
{
    [TestFixture]
    class ContactTests
    {
        [Test]
        [Category("Unit")]
        public void CreateContactTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n  \"id\": \"5673ee4ab7606abc252dade5\",\r\n  \"firstName\": \"Nick\",\r\n  \"lastName\": \"Davis\",\r\n  \"mobile\": \"+306984512555\",\r\n  \"country\": \"GR\",\r\n  \"vip\": false,\r\n  \"groups\": [\r\n    \"All\",\r\n    \"NotListed\"\r\n  ],\r\n  \"labels\": []\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);
            Contact contact=new Contact();
            contact.firstName = "Nick";
            contact.lastName = "Davis";
            contact.mobile = "+306984512555";
            contact.vip = false;
            RouteResponse<ContactResponse> contactResponse = manager.CreateContact(contact);

            Assert.NotNull(contactResponse);
            Assert.True(contactResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, contactResponse.StatusCode);

            Assert.AreEqual("5673ee4ab7606abc252dade5", contactResponse.Response.id);
            Assert.AreEqual("Nick", contactResponse.Response.firstName);
            Assert.AreEqual("Davis", contactResponse.Response.lastName);
            Assert.AreEqual("+306984512555", contactResponse.Response.mobile);
            Assert.AreEqual("GR", contactResponse.Response.country);
            Assert.AreEqual(false, contactResponse.Response.vip);
            Assert.AreEqual("All", contactResponse.Response.groups[0]);
            Assert.AreEqual("NotListed", contactResponse.Response.groups[1]);
        }

        [Test]
        [Category("Unit")]
        public void DeleteMultipleContacts()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "[\"5673fd9ce4b03d651b5b640f\", \"5673fda8e4b03d651b5b6410\"]"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);
            List<string> contactIds = (new[] {"5673fd9ce4b03d651b5b640f", "5673fda8e4b03d651b5b6410"}).ToList();
            RouteResponse<List<string>> contactDeleteResponse = manager.DeleteMultipleContacts(contactIds);

            Assert.NotNull(contactDeleteResponse);
            Assert.True(contactDeleteResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, contactDeleteResponse.StatusCode);

            Assert.AreEqual(2, contactDeleteResponse.Response.Count);
            Assert.AreEqual("5673fd9ce4b03d651b5b640f", contactDeleteResponse.Response[0]);
            Assert.AreEqual("5673fda8e4b03d651b5b6410", contactDeleteResponse.Response[1]);
            
        }

        [Test]
        [Category("Unit")]
        public void RetrieveAllContactsTest()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"content\":[  \r\n      {  \r\n         \"id\":\"5677be63e4b03e664ce6542f\",\r\n         \"firstName\":\"Nick2\",\r\n         \"lastName\":\"Davis\",\r\n         \"mobile\":\"+306984512666\",\r\n         \"country\":\"GR\",\r\n         \"vip\":false,\r\n         \"groups\":[  \r\n            \"All\",\r\n            \"one-two-three\",\r\n            \"group-difference2\",\r\n            \"group-difference\",\r\n            \"three\"\r\n         ],\r\n         \"blacklistedServices\":[  \r\n\r\n         ],\r\n         \"labels\":[  \r\n\r\n         ]\r\n      },\r\n      {  \r\n         \"id\":\"5674121de4b03d651b5b6414\",\r\n         \"firstName\":\"Nick\",\r\n         \"lastName\":\"Davis\",\r\n         \"mobile\":\"+306984512555\",\r\n         \"country\":\"GR\",\r\n         \"vip\":false,\r\n         \"groups\":[  \r\n            \"All\",\r\n            \"one-two-three\",\r\n            \"group-difference2\",\r\n            \"group-difference\",\r\n            \"two\"\r\n         ],\r\n         \"blacklistedServices\":[  \r\n            \"Sms\"\r\n         ],\r\n         \"labels\":[  \r\n\r\n         ]\r\n      },\r\n      {  \r\n         \"id\":\"56740e83e4b03d651b5b6413\",\r\n         \"firstName\":\"Nick3\",\r\n         \"lastName\":\"Davis\",\r\n         \"mobile\":\"+306984512777\",\r\n         \"country\":\"GR\",\r\n         \"vip\":false,\r\n         \"groups\":[  \r\n            \"All\",\r\n            \"one-two-three\",\r\n            \"group-difference2\",\r\n            \"one\",\r\n            \"group-difference\"\r\n         ],\r\n         \"blacklistedServices\":[  \r\n\r\n         ],\r\n         \"labels\":[  \r\n\r\n         ]\r\n      }\r\n   ],\r\n   \"totalPages\":1,\r\n   \"last\":true,\r\n   \"totalElements\":3,\r\n   \"numberOfElements\":3,\r\n   \"first\":true,\r\n   \"size\":20,\r\n   \"number\":0\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);
            RouteResponse<ContactList> contactList = manager.RetrieveAllContacts();

            Assert.NotNull(contactList);
            Assert.True(contactList.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, contactList.StatusCode);

            Assert.AreEqual(1m, contactList.Response.totalPages);
            Assert.AreEqual(3m, contactList.Response.totalElements);
            Assert.AreEqual(3m, contactList.Response.numberOfElements);
            Assert.AreEqual(20m, contactList.Response.size);
            Assert.AreEqual(0m, contactList.Response.number);
            Assert.True(contactList.Response.last);
            Assert.True(contactList.Response.first);

            Assert.AreEqual("5674121de4b03d651b5b6414", contactList.Response.content[1].id);
            Assert.AreEqual("Nick", contactList.Response.content[1].firstName);
            Assert.AreEqual("Davis", contactList.Response.content[1].lastName);
            Assert.AreEqual("+306984512555", contactList.Response.content[1].mobile);
            Assert.AreEqual("GR", contactList.Response.content[1].country);
            Assert.AreEqual(false, contactList.Response.content[1].vip);
            Assert.AreEqual(5, contactList.Response.content[1].groups.Count);
            Assert.AreEqual("Sms", contactList.Response.content[1].blacklistedServices[0]);
        }

        [Test]
        [Category("Unit")]
        public void RetrieveContactDetails()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"id\":\"5673fdbde4b03d651b5b6411\",\r\n   \"firstName\":\"Nick3\",\r\n   \"lastName\":\"Davis3\",\r\n   \"mobile\":\"+306984512777\",\r\n   \"country\":\"GR\",\r\n   \"vip\":false,\r\n   \"groups\":[  \r\n      \"All\",\r\n      \"NotListed\"\r\n   ],\r\n   \"blacklistedServices\":[  \r\n\r\n   ],\r\n   \"labels\":[  \r\n\r\n   ]\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);
            RouteResponse<ContactResponse> contactResponse = manager.RetrieveContactDetails("5673fdbde4b03d651b5b6411");

            Assert.NotNull(contactResponse);
            Assert.True(contactResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, contactResponse.StatusCode);

            Assert.AreEqual("5673fdbde4b03d651b5b6411", contactResponse.Response.id);
            Assert.AreEqual("Nick3", contactResponse.Response.firstName);
            Assert.AreEqual("Davis3", contactResponse.Response.lastName);
            Assert.AreEqual("+306984512777", contactResponse.Response.mobile);
            Assert.AreEqual("GR", contactResponse.Response.country);
            Assert.False(contactResponse.Response.vip);
            Assert.AreEqual(2, contactResponse.Response.groups.Count);
        }

        [Test]
        [Category("Unit")]
        public void UpdateContact()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"id\":\"5673fdbde4b03d651b5b6411\",\r\n   \"firstName\":\"James\",\r\n   \"lastName\":\"Davis\",\r\n   \"mobile\":\"+306984512999\",\r\n   \"country\":\"GR\",\r\n   \"vip\":false,\r\n   \"groups\":[  \r\n      \"All\",\r\n      \"NotListed\"\r\n   ],\r\n   \"blacklistedServices\":[  \r\n\r\n   ],\r\n   \"labels\":[  \r\n\r\n   ]\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);
            Contact contact = new Contact();
            contact.firstName = "James";
            contact.lastName = "Davis";
            contact.mobile = "+306984512999";
            contact.vip = false;
            
            RouteResponse<ContactResponse> updatedContactResponse = manager.UpdateContactDetails("5673fdbde4b03d651b5b6411", contact);

            Assert.NotNull(updatedContactResponse);
            Assert.True(updatedContactResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, updatedContactResponse.StatusCode);

            Assert.AreEqual("5673fdbde4b03d651b5b6411", updatedContactResponse.Response.id);
            Assert.AreEqual("James", updatedContactResponse.Response.firstName);
            Assert.AreEqual("Davis", updatedContactResponse.Response.lastName);
            Assert.AreEqual("+306984512999", updatedContactResponse.Response.mobile);
            Assert.AreEqual("GR", updatedContactResponse.Response.country);
            Assert.AreEqual(false, updatedContactResponse.Response.vip);
            Assert.AreEqual("All", updatedContactResponse.Response.groups[0]);
            Assert.AreEqual("NotListed", updatedContactResponse.Response.groups[1]);
        }

        [Test]
        [Category("Unit")]
        public void DeleteContact()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"id\":\"5673fdbde4b03d651b5b6411\",\r\n   \"firstName\":\"James\",\r\n   \"lastName\":\"Davis\",\r\n   \"mobile\":\"+306984512999\",\r\n   \"country\":\"GR\",\r\n   \"vip\":false,\r\n   \"groups\":[  \r\n      \"All\",\r\n      \"NotListed\"\r\n   ],\r\n   \"blacklistedServices\":[  \r\n\r\n   ],\r\n   \"labels\":[  \r\n\r\n   ]\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);
            Contact contact = new Contact();
            contact.firstName = "James";
            contact.lastName = "Davis";
            contact.mobile = "+306984512999";
            contact.vip = false;

            RouteResponse<ContactResponse> deletedContactResponse = manager.DeleteContact("5673fdbde4b03d651b5b6411");

            Assert.NotNull(deletedContactResponse);
            Assert.True(deletedContactResponse.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, deletedContactResponse.StatusCode);

            Assert.AreEqual("5673fdbde4b03d651b5b6411", deletedContactResponse.Response.id);
            Assert.AreEqual("James", deletedContactResponse.Response.firstName);
            Assert.AreEqual("Davis", deletedContactResponse.Response.lastName);
            Assert.AreEqual("+306984512999", deletedContactResponse.Response.mobile);
            Assert.AreEqual("GR", deletedContactResponse.Response.country);
            Assert.AreEqual(false, deletedContactResponse.Response.vip);
            Assert.AreEqual("All", deletedContactResponse.Response.groups[0]);
            Assert.AreEqual("NotListed", deletedContactResponse.Response.groups[1]);
        }

        [Test]
        [Category("Unit")]
        public void CheckIfContactExist()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);
            HttpStatusCode contactExist = manager.CheckIfContactExist("+306984512777");
            Assert.AreEqual(HttpStatusCode.OK, contactExist);
        }

        [Test]
        [Category("Unit")]
        public void AddContactToBlackList()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"updated\": 1\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);
            List<string> contactIds = (new[] {"5674121de4b03d651b5b6414"}).ToList();
            RouteResponse< UpdatedNumber > conatactResponses = manager.AddContactsToBlackList(ServiceType.Sms.ToString(), contactIds);

            Assert.NotNull(conatactResponses);
            Assert.True(conatactResponses.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, conatactResponses.StatusCode);

            Assert.AreEqual(1 ,conatactResponses.Response.updated);


        }

        [Test]
        [Category("Unit")]
        public void GetBlackListedContactForService()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "[  \r\n   {  \r\n      \"id\":\"5674121de4b03d651b5b6414\",\r\n      \"firstName\":\"Nick4\",\r\n      \"lastName\":\"Davis4\",\r\n      \"mobile\":\"+306984512555\",\r\n      \"country\":\"GR\",\r\n      \"vip\":false,\r\n      \"groups\":[  \r\n         \"All\",\r\n         \"NotListed\"\r\n      ],\r\n      \"blacklistedServices\":[  \r\n         \"Sms\"\r\n      ],\r\n      \"labels\":[  \r\n\r\n      ]\r\n   },\r\n   {  \r\n      \"id\":\"5674121de4b03d651b5b6515\",\r\n      \"firstName\":\"Nick5\",\r\n      \"lastName\":\"Davis5\",\r\n      \"mobile\":\"+306984512554\",\r\n      \"country\":\"GR\",\r\n      \"vip\":false,\r\n      \"groups\":[  \r\n         \"All\",\r\n         \"NotListed\"\r\n      ],\r\n      \"blacklistedServices\":[  \r\n         \"Sms\"\r\n      ],\r\n      \"labels\":[  \r\n\r\n      ]\r\n   }\r\n]"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);
            
            RouteResponse<List<ContactResponse>> conatactResponses = manager.GetBlackListedContactForAService(ServiceType.Sms.ToString());

            Assert.NotNull(conatactResponses);
            Assert.True(conatactResponses.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, conatactResponses.StatusCode);

            Assert.AreEqual(2, conatactResponses.Response.Count);

            Assert.AreEqual("5674121de4b03d651b5b6414", conatactResponses.Response[0].id);
            Assert.AreEqual("Nick4", conatactResponses.Response[0].firstName);
            Assert.AreEqual("Davis4", conatactResponses.Response[0].lastName);
            Assert.AreEqual("+306984512555", conatactResponses.Response[0].mobile);
            Assert.AreEqual("GR", conatactResponses.Response[0].country);
            Assert.False(conatactResponses.Response[0].vip);
            Assert.AreEqual(2, conatactResponses.Response[0].groups.Count);
            Assert.AreEqual(ServiceType.Sms.ToString(), conatactResponses.Response[0].blacklistedServices[0]);

            Assert.AreEqual("5674121de4b03d651b5b6515", conatactResponses.Response[1].id);
            Assert.AreEqual("Nick5", conatactResponses.Response[1].firstName);
            Assert.AreEqual("Davis5", conatactResponses.Response[1].lastName);
            Assert.AreEqual("+306984512554", conatactResponses.Response[1].mobile);
            Assert.AreEqual("GR", conatactResponses.Response[1].country);
            Assert.False(conatactResponses.Response[1].vip);
            Assert.AreEqual(2, conatactResponses.Response[1].groups.Count);
            Assert.AreEqual(ServiceType.Sms.ToString(), conatactResponses.Response[1].blacklistedServices[0]);


        }

        [Test]
        [Category("Unit")]
        public void RetrieveContactLabels()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"firstName\":\"Text\",\r\n   \"lastName\":\"Text\",\r\n   \"country\":\"Text\",\r\n   \"email\":\"Text\",\r\n   \"mobile\":\"Text\",\r\n   \"age\":12\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<Dictionary<string,string>> contactLabels = manager.RetrieveAccountContactLables();

            Assert.NotNull(contactLabels);
            Assert.True(contactLabels.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, contactLabels.StatusCode);

            Assert.AreEqual(6, contactLabels.Response.Count);
            Assert.AreEqual("Text", contactLabels.Response["firstName"]);
            Assert.AreEqual("Text", contactLabels.Response["lastName"]);
            Assert.AreEqual("Text", contactLabels.Response["country"]);
            Assert.AreEqual("Text", contactLabels.Response["email"]);
            Assert.AreEqual("Text", contactLabels.Response["mobile"]);
            Assert.AreEqual("12", contactLabels.Response["age"]);

        }

        [Test]
        [Category("Unit")]
        public void CreateLabels()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "[ { \"name\": \"age\", \"type\": \"Number\" } ]"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            ContactLabel label=new ContactLabel();
            label.name = "age";
            label.type = LabelType.Number.ToString();

            List<ContactLabel> labels=new List<ContactLabel>();
            labels.Add(label);
            RouteResponse<List<ContactLabel>> contactLabels = manager.CreateLables(labels);

            Assert.NotNull(contactLabels);
            Assert.True(contactLabels.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, contactLabels.StatusCode);

            Assert.AreEqual(1, contactLabels.Response.Count);
            Assert.AreEqual("age", contactLabels.Response[0].name);
            Assert.AreEqual("Number", contactLabels.Response[0].type);

        }

        [Test]
        [Category("Unit")]
        public void RetrieveAccountGroups()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "[  \r\n   {  \r\n      \"name\":\"All\"\r\n   },\r\n   {  \r\n      \"name\":\"NotListed\"\r\n   },\r\n   {  \r\n      \"name\":\"someGroup\"\r\n   }\r\n]"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            ContactLabel label = new ContactLabel();
            label.name = "age";
            label.type = LabelType.Number.ToString();

            List<ContactLabel> labels = new List<ContactLabel>();
            labels.Add(label);
            RouteResponse<List<AccountGroupName>> accountGroups = manager.RetrieveAccountGroups();

            Assert.NotNull(accountGroups);
            Assert.True(accountGroups.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, accountGroups.StatusCode);

            Assert.AreEqual(3, accountGroups.Response.Count);
            Assert.AreEqual("All", accountGroups.Response[0].name);
            Assert.AreEqual("NotListed", accountGroups.Response[1].name);
            Assert.AreEqual("someGroup", accountGroups.Response[2].name);
            
        }

        [Test]
        [Category("Unit")]
        public void RetrieveAccountGroupByName()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n   \"name\":\"All\"\r\n ,  \"size\":451\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);
           
            RouteResponse<AccountGroupSize> accountGroup = manager.RetrieveAccountGroupByName("All");

            Assert.NotNull(accountGroup);
            Assert.True(accountGroup.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, accountGroup.StatusCode);

            Assert.AreEqual("All", accountGroup.Response.name);
            Assert.AreEqual(451, accountGroup.Response.size);

        }
        
        [Test]
        [Category("Unit")]
        public void RetrieveAccountGroupPaged()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"totalPages\":1,\r\n   \"last\":true,\r\n   \"totalElements\":3,\r\n   \"first\":true,\r\n   \"numberOfElements\":3,\r\n   \"size\":3,\r\n   \"number\":0,\r\n   \"content\":[  \r\n      {\r\n         \"name\":\"All\",\r\n         \"size\":2\r\n      },\r\n      {     \r\n         \"name\":\"NotListed\",\r\n         \"size\":2\r\n      },\r\n      {     \r\n         \"name\":\"someGroup\",\r\n         \"size\":0\r\n      }\r\n   ]}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            RouteResponse<PagedAccountGroup> accountGroup = manager.RetrieveAccountGroupInPageFormat(0,3);

            Assert.NotNull(accountGroup);
            Assert.True(accountGroup.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, accountGroup.StatusCode);

            Assert.AreEqual(1m, accountGroup.Response.totalPages);
            Assert.AreEqual(true, accountGroup.Response.last);
            Assert.AreEqual(3m, accountGroup.Response.totalElements);
            Assert.AreEqual(true, accountGroup.Response.first);
            Assert.AreEqual(3m, accountGroup.Response.numberOfElements);
            Assert.AreEqual(3m, accountGroup.Response.size);
            Assert.AreEqual(0m, accountGroup.Response.number);
            Assert.AreEqual(3, accountGroup.Response.content.Count);
            Assert.AreEqual("All", accountGroup.Response.content[0].name);
            Assert.AreEqual(2, accountGroup.Response.content[0].size);
            Assert.AreEqual("NotListed", accountGroup.Response.content[1].name);
            Assert.AreEqual(2, accountGroup.Response.content[1].size);
            Assert.AreEqual("someGroup", accountGroup.Response.content[2].name);
            Assert.AreEqual(0, accountGroup.Response.content[2].size);


        }

        [Test]
        [Category("Unit")]
        public void CreateNewGroup()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n  \"name\": \"group1\",\r\n  \"size\": 0\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            AccountGroup accountGroup=new AccountGroup();
            accountGroup.name = "group1";
            RouteResponse<AccountGroupSize> accountGroupSize = manager.CreateNewGroup(accountGroup);

            Assert.NotNull(accountGroupSize);
            Assert.True(accountGroupSize.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, accountGroupSize.StatusCode);

            Assert.AreEqual("group1", accountGroupSize.Response.name);
            Assert.AreEqual(0, accountGroupSize.Response.size);

        }

        [Test]
        [Category("Unit")]
        public void DeleteGroups()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "[  \r\n   {  \r\n      \"name\":\"group1\",\r\n      \"deletedContacts\":false\r\n   },\r\n   \r\n]"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            List<string> groupNames=new List<string>();
            groupNames.Add("group1");
            RouteResponse<List<DeletedContacts>> deletedContacts = manager.DeleteGroups(groupNames,false);

            Assert.NotNull(deletedContacts);
            Assert.True(deletedContacts.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, deletedContacts.StatusCode);

            Assert.AreEqual(1, deletedContacts.Response.Count);
            Assert.AreEqual("group1", deletedContacts.Response[0].name);
            Assert.AreEqual(false, deletedContacts.Response[0].deletedContacts);

        }

        [Test]
        [Category("Unit")]
        public void MergeMultipleGroups()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"name\":\"one-two-three\",\r\n   \"size\":3\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            List<string> groupNames = new List<string>();
            groupNames.Add("one");
            groupNames.Add("two");
            groupNames.Add("three");
            RouteResponse<AccountGroupSize> mergedGroup = manager.MergeMultipleGroups("one-two-three", groupNames);

            Assert.NotNull(mergedGroup);
            Assert.True(mergedGroup.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, mergedGroup.StatusCode);
            
            Assert.AreEqual("one-two-three", mergedGroup.Response.name);
            Assert.AreEqual(3, mergedGroup.Response.size);

        }

        [Test]
        [Category("Unit")]
        public void CreateGroupFromDifference()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"name\":\"difference\",\r\n   \"size\":4\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            List<string> groupNames = new List<string>();
            groupNames.Add("one");
            groupNames.Add("two");
            
            RouteResponse<AccountGroupSize> difference = manager.CreateGroupFromDifferences("difference", groupNames);

            Assert.NotNull(difference);
            Assert.True(difference.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, difference.StatusCode);

            Assert.AreEqual("difference", difference.Response.name);
            Assert.AreEqual(4, difference.Response.size);

        }

        [Test]
        [Category("Unit")]
        public void ViewContactsOfSpecificGroup()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"content\":[  \r\n      {  \r\n         \"id\":\"5677be63e4b03e664ce6542f\",\r\n         \"firstName\":\"Nick2\",\r\n         \"lastName\":\"Davis\",\r\n         \"mobile\":\"+306984512666\",\r\n         \"country\":\"GR\",\r\n         \"vip\":false,\r\n         \"groups\":[  \r\n            \"All\",\r\n            \"one-two-three\",\r\n            \"group-difference2\",\r\n            \"group-difference\",\r\n            \"three\"\r\n         ],\r\n         \"blacklistedServices\":[  \r\n\r\n         ],\r\n         \"labels\":[  \r\n\r\n         ]\r\n      },\r\n      {  \r\n         \"id\":\"5674121de4b03d651b5b6414\",\r\n         \"firstName\":\"Nick\",\r\n         \"lastName\":\"Davis\",\r\n         \"mobile\":\"+306984512555\",\r\n         \"country\":\"GR\",\r\n         \"vip\":false,\r\n         \"groups\":[  \r\n            \"All\",\r\n            \"one-two-three\",\r\n            \"group-difference2\",\r\n            \"group-difference\",\r\n            \"two\"\r\n         ],\r\n         \"blacklistedServices\":[  \r\n            \"Sms\"\r\n         ],\r\n         \"labels\":[  \r\n\r\n         ]\r\n      },\r\n      {  \r\n         \"id\":\"56740e83e4b03d651b5b6413\",\r\n         \"firstName\":\"Nick3\",\r\n         \"lastName\":\"Davis\",\r\n         \"mobile\":\"+306984512777\",\r\n         \"country\":\"GR\",\r\n         \"vip\":false,\r\n         \"groups\":[  \r\n            \"All\",\r\n            \"one-two-three\",\r\n            \"group-difference2\",\r\n            \"one\",\r\n            \"group-difference\"\r\n         ],\r\n         \"blacklistedServices\":[  \r\n\r\n         ],\r\n         \"labels\":[  \r\n\r\n         ]\r\n      }\r\n   ],\r\n   \"totalPages\":1,\r\n   \"last\":true,\r\n   \"totalElements\":3,\r\n   \"numberOfElements\":3,\r\n   \"first\":true,\r\n   \"size\":20,\r\n   \"number\":0\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            List<string> groupNames = new List<string>();
            groupNames.Add("one");
            groupNames.Add("two");

            RouteResponse<ContactList> contacts = manager.GetContactsOfSpecificGroup("one-two-three");

            Assert.NotNull(contacts);
            Assert.True(contacts.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, contacts.StatusCode);

            Assert.AreEqual(3, contacts.Response.content.Count);
            Assert.AreEqual("5677be63e4b03e664ce6542f", contacts.Response.content[0].id);
            Assert.AreEqual("Nick2", contacts.Response.content[0].firstName);
            Assert.AreEqual("Davis", contacts.Response.content[0].lastName);
            Assert.AreEqual("+306984512666", contacts.Response.content[0].mobile);
            Assert.AreEqual("GR", contacts.Response.content[0].country);
            Assert.AreEqual(false, contacts.Response.content[0].vip);
            Assert.AreEqual(5, contacts.Response.content[0].groups.Count);

        }

        [Test]
        [Category("Unit")]
        public void DeleteContactsOfSpecificGroup()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"name\":\"customers\",\r\n   \"size\":3\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            List<string> contactIds = new List<string>();
            contactIds.Add("5673fdbde4b03d651b5b6411");
            contactIds.Add("5673fdbde4b03d651b5b6412");

            RouteResponse<AccountGroupSize> deletedContacts = manager.DeleteContactsOfSpecificGroup("customers",contactIds);

            Assert.NotNull(deletedContacts);
            Assert.True(deletedContacts.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, deletedContacts.StatusCode);

            Assert.AreEqual("customers", deletedContacts.Response.name);
            Assert.AreEqual(3, deletedContacts.Response.size);
        }

        [Test]
        [Category("Unit")]
        public void AddContactsSpecificGroup()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{  \r\n   \"name\":\"one-two-three\",\r\n   \"size\":3\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            List<string> contactIds = new List<string>();
            contactIds.Add("5677c20de4b03e664ce65430");
            contactIds.Add("5677d9f5e4b03e664ce65431");
            contactIds.Add("5677d9fde4b03e664ce6543");

            RouteResponse<AccountGroupSize> addedContacts = manager.DeleteContactsOfSpecificGroup("one-two-three", contactIds);

            Assert.NotNull(addedContacts);
            Assert.True(addedContacts.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, addedContacts.StatusCode);

            Assert.AreEqual("one-two-three", addedContacts.Response.name);
            Assert.AreEqual(3, addedContacts.Response.size);
        }

        [Test]
        [Category("Unit")]
        public void RemoveContactsFromBlackList()
        {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = "{\r\n      \"updated\":\"1\"\r\n}"
                });

            SdkManager manager = new SdkManager("", "", restClient.Object);

            List<string> contactIds = new List<string>();
            contactIds.Add("5674121de4b03d651b5b6414");

            RouteResponse<UpdatedNumber> removedContacts = manager.RemoveContactsFromServiceBlackList("one-two-three", contactIds);

            Assert.NotNull(removedContacts);
            Assert.True(removedContacts.HasValue);
            Assert.AreEqual(HttpStatusCode.OK, removedContacts.StatusCode);

            Assert.AreEqual(1, removedContacts.Response.updated);
            
        }
    }
}
