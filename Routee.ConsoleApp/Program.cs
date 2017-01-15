using Routee.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Routee.Sdk.Catalogs;
using Routee.Sdk.Models;

namespace Routee.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SdkManager manager = new SdkManager("APP ID", "Secret");

            //STEP 1 Get Authorized
            RouteResponse<LoginResponse> response = manager.Login();
            
            if (response.HasValue)
            {
                Console.WriteLine("Login Successfullll");
                Console.WriteLine("Access token= " + response.Response.access_token);

                //NOTE: Uncomment use cases to execute them
                //UseCase1(manager);
                //UseCase2(manager);
                //UseCase3(manager);
                //UseCase4(manager);
                UseCase5(manager);
            }

            Console.ReadKey();
        }

        private static void UseCase1(SdkManager manager)
        {
            //STEP 2 & 3: Create Labels
            ContactLabel numberlabel = new ContactLabel() {name = "cats", type = LabelType.Number.ToString()};
            ContactLabel textlabel = new ContactLabel() {name = "address", type = LabelType.Text.ToString()};
            var labelResult = manager.CreateLables(new[] {numberlabel, textlabel}.ToList());
            if (labelResult.HasValue)
            {
                foreach (var contactLabel in labelResult.Response)
                {
                    Console.WriteLine("Label Created,  Name:{0}, Type:{1}", contactLabel.name, contactLabel.type);
                }
            }
            else
            {
                DisplayError(labelResult.Error, "CreateLables");
            }

            //STEP 4: Create contact
            Contact contact = new Contact();
            contact.email = "email@email.com";
            contact.firstName = "firstname";
            contact.lastName = "lastname";
            contact.mobile = "+306973359355";
            List<Label> labels = new List<Label>();
            labels.Add(new Label() {name = "cats", value = 5});
            labels.Add(new Label() {name = "address", value = "SomeAddress"});
            contact.labels = labels;
            var contactRespons = manager.CreateContact(contact);
            if (contactRespons.HasValue)
            {
                Console.WriteLine("Contact created, ID:{0}", contactRespons.Response.id);
            }
            else
            {
                DisplayError(contactRespons.Error, "CreateContact");
            }

            //STEP5: Create group
            AccountGroup group = new AccountGroup();
            group.name = "PeopleWithCats";
            var groupResponse = manager.CreateNewGroup(group);

            if (groupResponse.HasValue)
            {
                Console.WriteLine("Group created, Naem:{0}, Size:{1}", groupResponse.Response.name, groupResponse.Response.size);
            }
            else
            {
                DisplayError(groupResponse.Error, "CreateNewGroup");
            }


            //STEP 6: Add Contact to group
            var added = manager.AddContactsToSpecificGroup(group.name, new[] {contactRespons.Response.id}.ToList());

            if (added.HasValue)
            {
                Console.WriteLine("Added, Group Size:" + added.Response.size);
            }
            else
            {
                DisplayError(added.Error, "AddContactsToSpecificGroup");
            }

            //STEP 7: Retreive contact details
            var contactDetails = manager.RetrieveContactDetails(contactRespons.Response.id);
            if (contactDetails.HasValue)
            {
                Console.WriteLine("Groups of contacts are:");
                foreach (var responseGroup in contactDetails.Response.groups)
                {
                    Console.WriteLine(responseGroup);
                }
            }
            else
            {
                DisplayError(contactDetails.Error, "RetrieveContactDetails");
            }
        }

        private static void UseCase2(SdkManager manager)
        {
            // STEP 2 send sms
            SingleSms singleSms=new SingleSms();
            singleSms.body = "sample body";
            singleSms.from = "amdTelecom";
            singleSms.to = "+306973359355";
            var singlesmsResponse=manager.SendSms(singleSms);
            if (singlesmsResponse.HasValue)
            {
                Console.WriteLine("Sms sent, trackingID" + singlesmsResponse.Response.trackingId + ", Status" +
                                  singlesmsResponse.Response.status);
            }
            else
            {
                DisplayError(singlesmsResponse.Error, "SendSms");
            }

            //STEP 3 track sms
            TrackMultipleSms multipleSms=new TrackMultipleSms();
            multipleSms.fieldName = "from";
            multipleSms.searchTerm = "amdTelecom";
            var tracksms = manager.TrackMultipleSmsWithFilters(new[] {multipleSms}.ToList());

            if (tracksms.HasValue)
            {
                foreach (var trackSmsResponse in tracksms.Response.content)
                {
                    Console.WriteLine("Sms found, Id:{0}, to:{1}, body:{2}, Status:{3}", trackSmsResponse.messageId, trackSmsResponse.to, trackSmsResponse.body, trackSmsResponse.status.status);
                }
            }
            else
            {
                DisplayError(tracksms.Error, "TrackMultipleSmsWithFilters");
            }
        }

        private static void UseCase3(SdkManager manager)
        {
            //STEP 2: Create contact
            Contact contact = new Contact();
            contact.email = "email@email.com";
            contact.firstName = "firstname";
            contact.lastName = "lastname";
            contact.mobile = "+306973359355";

            var contactRespons = manager.CreateContact(contact);
            if (contactRespons.HasValue)
            {
                Console.WriteLine("Contact created, ID:{0}", contactRespons.Response.id);
            }
            else
            {
                DisplayError(contactRespons.Error, "CreateContact");
            }


            //STEP3: Send campaign
            Campaign campaign=new Campaign();
            campaign.campaignName = "Campaign Name";
            campaign.from = "amdTelecom";
            campaign.to = new List<string>();
            campaign.to.Add("additional recepient number");
            campaign.body = "You body";
            campaign.contacts = new List<string>();
            campaign.contacts.Add(contactRespons.Response.id);
            campaign.scheduledDate=DateTime.Now;//run campaign now
            campaign.callback=new Callback();
            campaign.callback.url = "http://192.168.1.1";
            campaign.callback.strategy = CallbackStrategy.OnChange;//can be oncompletion also
            var campaignResponse = manager.SendSmsCampaign(campaign);
            if (campaignResponse.HasValue)
            {
                Console.Write("Campaign Scheduled, TrackingId:" + campaignResponse.Response.trackingId);
            }
            else
            {
                DisplayError(campaignResponse.Error, "SendSmsCampaign");
            }

            //STEP5: Retreive details of campaign
            var campaignDetails = manager.GetCampaignDetails(campaignResponse.Response.trackingId);
            if (campaignDetails.HasValue)
            {
                Console.WriteLine("Campaign Details: TrackingId:{0}, CampaignName:{1} ", campaignDetails.Response.trackingId, campaignDetails.Response.campaignName);
                //can log other properties to view them
            }
            else
            {
                DisplayError(campaignDetails.Error, "GetCampaignDetails");
            }

            //STEP6: Track message of campagin
            var trackCampaignMessage = manager.TrackMessagesOfACampaign(campaignResponse.Response.trackingId);
            if (trackCampaignMessage.HasValue)
            {
                foreach (var trackSmsResponse in trackCampaignMessage.Response.content)
                {
                    Console.WriteLine("Sms found, Id:{0}, to:{1}, body:{2}, Status:{3}", trackSmsResponse.messageId, trackSmsResponse.to, trackSmsResponse.body, trackSmsResponse.status.status);
                }
            }
            else
            {
                DisplayError(trackCampaignMessage.Error, "TrackMessagesOfACampaign");
            }
        }
        
        private static void UseCase4(SdkManager manager)
        {
            Contact contact = new Contact();
            contact.email = "email@email.com";
            contact.firstName = "firstname";
            contact.lastName = "lastname";
            contact.mobile = "+306973359355";

            var contactRespons = manager.CreateContact(contact);
            if (contactRespons.HasValue)
            {
                Console.WriteLine("Contact created, ID:{0}", contactRespons.Response.id);
            }
            else
            {
                DisplayError(contactRespons.Error, "CreateContact");
            }

            //STEP 2: Create a scheduled campaign
            Campaign campaign = new Campaign();
            campaign.campaignName = "Campaign Name";
            campaign.from = "amdTelecom";
            campaign.to = new List<string>();
            campaign.to.Add("+306973359356");
            campaign.body = "You body";
            campaign.contacts = new List<string>();
            campaign.contacts.Add(contactRespons.Response.id);
            campaign.scheduledDate = DateTime.Now.AddDays(3);//run after 3 days from now
            campaign.callback = new Callback();
            campaign.callback.url = "http://192.168.1.1";
            campaign.callback.strategy = CallbackStrategy.OnChange;//can be oncompletion also
            var campaignResponse = manager.SendSmsCampaign(campaign);
            if (campaignResponse.HasValue)
            {
                Console.Write("Campaign Scheduled, TrackingId:" + campaignResponse.Response.trackingId);
            }
            else
            {
                DisplayError(campaignResponse.Error, "SendSmsCampaign");
            }

            //STEP3: Delete campaign
            var deletedCampaign = manager.DeleteScheduledCampaign(campaignResponse.Response.trackingId);
            if (deletedCampaign.StatusCode==HttpStatusCode.OK)
            {
                Console.WriteLine("Campaign Deleted");
            }
            else
            {
                DisplayError(deletedCampaign.Error, "DeleteScheduledCampaign");
            }

            //STEP4: Track messages of campaign
            var trackCampaignMessage = manager.TrackMessagesOfACampaign(campaignResponse.Response.trackingId);
            if (trackCampaignMessage.HasValue)
            {
                foreach (var trackSmsResponse in trackCampaignMessage.Response.content)
                {
                    Console.WriteLine("Sms found, Id:{0}, to:{1}, body:{2}, Status:{3}", trackSmsResponse.messageId, trackSmsResponse.to, trackSmsResponse.body, trackSmsResponse.status.status);
                }
            }
            else
            {
                DisplayError(trackCampaignMessage.Error, "TrackMessagesOfACampaign");
            }
        }

        private static void UseCase5(SdkManager manager)
        {
            //STEP2: send two step verification request
            TwoStepRequest twoStepRequest=new TwoStepRequest();
            twoStepRequest.method = "sms";
            twoStepRequest.recipient = "+306973359355";
            twoStepRequest.type = "code";
            var twoStepResuestResponse = manager.SendTwoStepVerification(twoStepRequest);
            if (twoStepResuestResponse.HasValue)
            {
                Console.WriteLine("Two step created, TrackingId:{0}, Status:{1}, UpdatedAt:{2}",
                    twoStepResuestResponse.Response.trackingId, twoStepResuestResponse.Response.status,
                    twoStepResuestResponse.Response.updatedAt);
            }
            else
            {
                DisplayError(twoStepResuestResponse.Error, "SendTwoStepVerification");
            }

            //STEP3: Retreive two step verification status
            var twoStepVerificationStatus =
                manager.GetTwoStepVerificationStatus(twoStepResuestResponse.Response.trackingId);
            if (twoStepVerificationStatus.HasValue)
            {
                Console.WriteLine("Two Step Verification Staus:"+twoStepVerificationStatus.Response.status);
            }
            else
            {
                DisplayError(twoStepVerificationStatus.Error, "GetTwoStepVerificationStatus");
            }

            //STEP 4: Cancel two step verification
            var cancelVerificaation =
                manager.CancelTwoStepVerification(twoStepResuestResponse.Response.trackingId);
            if (cancelVerificaation.HasValue)
            {
                Console.WriteLine("Two Step Verification Staus after cancelling:" + cancelVerificaation.Response.status);
            }
            else
            {
                DisplayError(cancelVerificaation.Error, "CancelTwoStepVerification");
            }

            //STEP5: Confirm two step verification
            var confirmVerification =
                manager.ConfirmTwoStepVerification(twoStepResuestResponse.Response.trackingId,"code");
            if (confirmVerification.HasValue)
            {
                Console.WriteLine("Two Step Verification Staus after Confirming:" + confirmVerification.Response.status);
            }
            else
            {
                DisplayError(confirmVerification.Error, "ConfirmTwoStepVerification");
            }

        }

        private static void DisplayError(Error error, string method)
        {
            if (error != null)
            {
                Console.WriteLine("===============+"+method.ToUpper()+"==================");
                Console.WriteLine("Error while calling:{0}, ErrorCode:{1}, DeveloperMessage:{2}", method, error.code,
                    error.developerMessage);

                if (error.HasProperties)
                {
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("Errors are:");
                    foreach (var errorProperty in error.properties)
                    {
                        Console.WriteLine(errorProperty.Key + " : " + errorProperty.Value);
                    }
                    Console.WriteLine("-------------------------------------");
                }
                Console.WriteLine("========================================");
            }
        }
    }
}
