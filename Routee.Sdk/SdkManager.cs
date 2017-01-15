using Routee.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;
using Routee.Sdk.Catalogs;

namespace Routee.Sdk
{
    public class SdkManager
    {
        public string appId = "";
        public string secret = "";
        private string _token;

        private string _appId;
        private string _secret;

        private IRestClient _client;

        /// <summary>
        /// Constructor to be used as in normal situations
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="baseUrl"></param>
        public SdkManager(string appId, string secret, string baseUrl = "https://connect.routee.net")
        {
            _appId = appId;
            _secret = secret;
            _baseUrl = baseUrl;
            _client=new RestClient(baseUrl);
        }

        /// <summary>
        /// Constructor for injecting IREST Client
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="restClient"></param>
        /// <param name="baseUrl"></param>
        public SdkManager(string appId, string secret, IRestClient restClient, string baseUrl = "https://connect.routee.net")
        {
            _appId = appId;
            _secret = secret;
            _baseUrl = baseUrl;
            _client = restClient;
        }

        private string _baseUrl;

        /// <summary>
        /// Base64 Encoding
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="permissions">Permissions to be assigned to login session, default is all permissions</param>
        /// <returns>RouteResponse with LoginResponse as Response object in it</returns>
        public RouteResponse<LoginResponse> Login(List<string> permissions = null)
        {
            RouteResponse<LoginResponse> response = new RouteResponse<LoginResponse>();
            //var client = new RestClient("https://auth.routee.net");

            var request = new RestRequest("/oauth/token", Method.POST);

            //request.AddBody("grant_type=client_credentials");
            request.AddParameter("grant_type", "client_credentials");
            if (permissions != null && permissions.Any())
            {
                request.AddParameter("scope", string.Join(",", permissions));
            }
            request.RequestFormat = DataFormat.Json;


            // easily add HTTP Headers
            request.AddHeader("Authorization", "Basic " + Base64Encode(_appId + ":" + _secret));
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            IRestResponse restResponse = _client.Execute(request);
            response.StatusCode = restResponse.StatusCode;

            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                response.Response = JsonConvert.DeserializeObject<LoginResponse>(restResponse.Content);
                _token = response.Response.access_token;
            }

            else
            {
                response.Error = JsonConvert.DeserializeObject<Error>(restResponse.Content);
            }

            return response;
        }

        #region Messaging Section
        /// <summary>
        /// Send a single SMS
        /// </summary>
        /// <param name="single">Sinlgesms object</param>
        /// <returns>RouteResponse with SmsResponse as Response object in it</returns>
        public RouteResponse<SmsResponse> SendSms(SingleSms single)
        {
            return CallRoutee<SmsResponse, SingleSms>(single, _baseUrl, "sms");
        }

        /// <summary>
        /// Analyze a single message
        /// </summary>
        /// <param name="analyzeMessage">BaseSms</param>
        /// <returns>RouteResponse with AnalyzeMessageResponse as Response object in it</returns>
        public RouteResponse<AnalyzeMessageResponse> AnalyzeSingleSms(BaseSms analyzeMessage)
        {
            return CallRoutee<AnalyzeMessageResponse, BaseSms>(analyzeMessage, _baseUrl, "sms/analyze");
        }

        /// <summary>
        /// Send a sms campaign
        /// </summary>
        /// <param name="campaign">Campaign</param>
        /// <returns>RouteResponse with CampaignResponse as Response object in it</returns>
        public RouteResponse<CampaignResponse> SendSmsCampaign(Campaign campaign)
        {
            return CallRoutee<CampaignResponse, Campaign>(campaign, _baseUrl, "sms/campaign");
        }

        /// <summary>
        /// Analyze a campaign
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public RouteResponse<AnalyzeCampaignResponse> AnalyzeCampaign(AnalyzeCampaign campaign)
        {
            return CallRoutee<AnalyzeCampaignResponse, AnalyzeCampaign>(campaign, _baseUrl, "sms/analyze/campaign");
        }

        /// <summary>
        /// Track a single SMS
        /// </summary>
        /// <param name="messageId">Message Id</param>
        /// <returns>RouteResponse with List of TrackSmsResponse as Response object in it</returns>
        public RouteResponse<List<TrackSmsResponse>> TrackSingleMessage(string messageId)
        {
            return CallRouteeGet<List<TrackSmsResponse>>(_baseUrl, "sms/tracking/single/" + messageId);
        }

        /// <summary>
        /// Track multiple SMS of a specific campaign
        /// </summary>
        /// <param name="campaignTrackingId">Campaign Tracking Id</param>
        /// <param name="page">Number of pages</param>
        /// <param name="size">Size</param>
        /// <param name="sort">Sort field name</param>
        /// <returns>RouteResponse with TrackSmsCampaignResponse as Response object in it</returns>
        public RouteResponse<TrackSmsCampaignResponse> TrackMessagesOfACampaign(string campaignTrackingId, int page = 0,
            int size = 10, string sort = null)
        {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("page", page.ToString());
            queryParameters.Add("size", size.ToString());
            if (!string.IsNullOrEmpty(sort))
            {
                queryParameters.Add("sort", sort);
            }

            return CallRouteeGet<TrackSmsCampaignResponse>(_baseUrl, "sms/tracking/campaign/" + campaignTrackingId,
                queryParameters);
        }

        /// <summary>
        /// Track multiple sms with filters for a specific time range
        /// </summary>
        /// <param name="multipleSmses"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="sort"></param>
        /// <param name="trackingId"></param>
        /// <param name="campaign"></param>
        /// <returns>RouteResponse with TrackSmsCampaignResponse as Response object in it</returns>
        public RouteResponse<TrackSmsCampaignResponse> TrackMultipleSmsWithFilters(List<TrackMultipleSms> multipleSmses, DateTime? dateStart = null, DateTime? dateEnd = null, int page = 0,
            int size = 10, string sort = null, string trackingId = null, bool campaign = false)
        {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("page", page.ToString());
            queryParameters.Add("size", size.ToString());
            queryParameters.Add("campaign", campaign.ToString());

            if (!string.IsNullOrEmpty(sort))
            {
                queryParameters.Add("sort", sort);
            }
            if (!string.IsNullOrEmpty(trackingId))
            {
                queryParameters.Add("trackingId", trackingId);
            }
            if (dateStart.HasValue)
            {
                queryParameters.Add("dateStart", JsonConvert.SerializeObject(dateStart.Value));
            }
            if (dateEnd.HasValue)
            {
                queryParameters.Add("dateEnd", JsonConvert.SerializeObject(dateEnd.Value));
            }

            return CallRoutee<TrackSmsCampaignResponse, List<TrackMultipleSms>>(multipleSmses, _baseUrl, "sms/tracking", queryParameters);
        }


        /// <summary>
        /// Retrieve the countries that are supported by the quiet hours feature
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>RouteResponse with List of QuietHourCountry as Response object in it</returns>
        public RouteResponse<List<QuietHourCountry>> RetrieveQueitHourCountryList(string language)
        {
            return CallRouteeGet<List<QuietHourCountry>>(_baseUrl, "sms/quietHours/countries/" + language);
        }

        /// <summary>
        /// Update a scheduled message campaign
        /// </summary>
        /// <param name="campaignTrackingId">CampaignTrackingId</param>
        /// <param name="campaign">Updated Campaign</param>
        /// <returns>RouteResponse with CampaignResponse as Response object in it</returns>
        public RouteResponse<CampaignResponse> UpdateSmsCampaign(string campaignTrackingId, Campaign campaign)
        {
            return CallRoutee<CampaignResponse, Campaign>(campaign, _baseUrl, "sms/" + campaignTrackingId,
                httpMethod: Method.PUT);
        }

        /// <summary>
        /// Delete a scheduled campaign
        /// </summary>
        /// <param name="trackingId">TrackingId</param>
        /// <returns>RouteResponse with object as Response object in it, Check HttpStatusCode in reponse</returns>
        public RouteResponse<object> DeleteScheduledCampaign(string trackingId)
        {
            return CallRoutee<object, object>(null, _baseUrl, "sms/" + trackingId, httpMethod: Method.DELETE);
        }

        /// <summary>
        /// Retrieve Details for a campaign
        /// </summary>
        /// <param name="trackingId">Tracking Id</param>
        /// <returns>RouteResponse with CampaignResponse as Response object in it</returns>
        public RouteResponse<CampaignResponse> GetCampaignDetails(string trackingId)
        {
            return CallRouteeGet<CampaignResponse>(_baseUrl, "campaigns/" + trackingId);
        }

        #endregion

        #region Accounts Section

        /// <summary>
        /// Retrieve the balance of your account
        /// </summary>
        /// <returns>RouteResponse with BalanceInfo as Response object in it</returns>
        public RouteResponse<BalanceInfo> GetBalanceInformation()
        {
            return CallRouteeGet<BalanceInfo>(_baseUrl, "accounts/me/balance");
        }

        /// <summary>
        /// Retrieve the prices for all Routee services
        /// </summary>
        /// <param name="mcc">OPTIONAL:</param>
        /// <param name="mnc">OPTIONAL:</param>
        /// <param name="serviceType">OPTIONAL:</param>
        /// <param name="currency">OPTIONAL:</param>
        /// <returns>RouteResponse with RouteeServicesPrice as Response object in it</returns>
        public RouteResponse<RouteeServicesPrice> GetPricesOfAllServices(string mcc=null,string mnc=null,ServiceType? serviceType=null,string currency=null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(mcc))
            {
              parameters.Add("mcc",mcc); 
            }
            if (!string.IsNullOrEmpty(mnc))
            {
                parameters.Add("mnc", mnc);
            }
            if (serviceType.HasValue)
            {
                parameters.Add("service", serviceType.ToString());
            }
            if (!string.IsNullOrEmpty(currency))
            {
                parameters.Add("currency", currency);
            }
            return CallRouteeGet<RouteeServicesPrice>(_baseUrl, "system/prices", parameters);
        }


        /// <summary>
        /// Retrieve the transactions of your account
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>RouteResponse with TransactionsHistory as Response object in it</returns>
        public RouteResponse<TransactionsHistory> GetAccountTransactions(int page = 0, int size = 20, DateTime? from=null, DateTime? to=null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (from.HasValue)
            {
                parameters.Add("from", JsonConvert.SerializeObject(from.Value));
            }
            if (to.HasValue)
            {
                parameters.Add("to", JsonConvert.SerializeObject(to.Value));
            }
            parameters.Add("page", page.ToString());
            parameters.Add("size", size.ToString());
        
            return CallRouteeGet<TransactionsHistory>(_baseUrl, "accounts/me/transactions", parameters);
        }

        /// <summary>
        /// Retrieve the available bank accounts
        /// </summary>
        /// <returns>RouteResponse with BankAccount as Response object in it</returns>
        public RouteResponse<BankAccount> GetAvailableBankAccounts()
        {
            return CallRouteeGet<BankAccount>(_baseUrl, "accounts/me/banks");
        }
        #endregion

        #region 2StepVerfication Region

        /// <summary>
        /// Sending A 2Step verification
        /// </summary>
        /// <param name="request">TwoStepRequest</param>
        /// <returns>RouteResponse with TwoStepResponse as Response object in it</returns>
        public RouteResponse<TwoStepResponse> SendTwoStepVerification(TwoStepRequest request)
        {
            return CallRoutee<TwoStepResponse,TwoStepRequest>(request, _baseUrl, "2step");
        }

        /// <summary>
        /// Retrieving the status of a 2Step verification
        /// </summary>
        /// <param name="trackingId">TrackingId</param>
        /// <returns>RouteResponse with TwoStepResponse as Response object in it</returns>
        public RouteResponse<TwoStepResponse> GetTwoStepVerificationStatus(string trackingId)
        {
            return CallRouteeGet<TwoStepResponse>(_baseUrl, "2step/" + trackingId);
        }

        /// <summary>
        /// Cancel a 2Step verification
        /// </summary>
        /// <param name="trackingId">TrackingId</param>
        /// <returns>RouteResponse with TwoStepResponse as Response object in it</returns>
        public RouteResponse<TwoStepResponse> CancelTwoStepVerification(string trackingId)
        {
            return CallRoutee<TwoStepResponse, object>(null, _baseUrl, "2step/" + trackingId, httpMethod: Method.DELETE);
        }

        /// <summary>
        /// Confirm a 2Step verification
        /// </summary>
        /// <param name="trackingId">TrackingId</param>
        /// <param name="answer">Answer/code received</param>
        /// <returns>RouteResponse with TwoStepResponse as Response object in it</returns>
        public RouteResponse<TwoStepResponse> ConfirmTwoStepVerification(string trackingId, string answer)
        {
            Dictionary<string,string> formencoded=new Dictionary<string, string>();
            formencoded.Add("answer",answer);
            return CallRouteeFormUrlEncoded<TwoStepResponse>(_baseUrl, "2step/" + trackingId, formEncoded:formencoded);
        }

        /// <summary>
        /// Retrieve 2Step account reports
        /// </summary>
        /// <returns>RouteResponse with TwoStepAccountHistoryReport as Response object in it</returns>
        public RouteResponse<TwoStepAccountHistoryReport> Get2StepAccountReports()
        {
            return CallRouteeGet<TwoStepAccountHistoryReport>(_baseUrl, "2step/reports");
        }

        /// <summary>
        /// Retrieve 2Step application reports
        /// </summary>
        /// <param name="applicationId">ApplicationId for which report is desired</param>
        /// <returns>RouteResponse with TwoStepAccountHistoryReport as Response object in it</returns>
        public RouteResponse<TwoStepAccountHistoryReport> Get2StepApplicationReports(string applicationId)
        {
            return CallRouteeGet<TwoStepAccountHistoryReport>(_baseUrl, "2step/reports/applications/" + applicationId);
        }
        #endregion

        #region Reports

        /// <summary>
        /// View Volume/Price summary analytics for a range of messages
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>RouteResponse with list of ReportResult as Response object in it</returns>
        public RouteResponse<List<ReportResult>> ViewVolumePriceAnalytics(DateTime startDate, DateTime endDate)
        {
            Dictionary<string, string> query = new Dictionary<string, string>();

            query.Add("startDate", JsonConvert.SerializeObject(startDate));
            query.Add("endDate", JsonConvert.SerializeObject(endDate));
            return CallRouteeGet<List<ReportResult>>(_baseUrl, "reports/my/volPrice",query);
        }

        /// <summary>
        /// View Volume/Price summary analytics for a Country
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="mcc"></param>
        /// <returns>RouteResponse with list of ReportResult as Response object in it</returns>
        public RouteResponse<List<ReportResult>> ViewVolumePriceAnalyticsForACountry(DateTime startDate, DateTime endDate, string mcc)
        {
            Dictionary<string, string> query = new Dictionary<string, string>();

            query.Add("startDate", JsonConvert.SerializeObject(startDate));
            query.Add("endDate", JsonConvert.SerializeObject(endDate));
            query.Add("mcc",mcc);
            return CallRouteeGet<List<ReportResult>>(_baseUrl, "reports/my/volPrice/perMcc", query);
        }

        /// <summary>
        /// View Volume/Price summary analytics for a Country and Network
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="mcc"></param>
        /// <param name="mnc"></param>
        /// <returns>RouteResponse with list of ReportResult as Response object in it</returns>
        public RouteResponse<List<ReportResult>> ViewVolumePriceAnalyticsForACountryAndNetwork(DateTime startDate, DateTime endDate, string mcc, string mnc)
        {
            Dictionary<string, string> query = new Dictionary<string, string>();

            query.Add("startDate", JsonConvert.SerializeObject(startDate));
            query.Add("endDate", JsonConvert.SerializeObject(endDate));
            query.Add("mcc", mcc);
            query.Add("mnc", mnc);
            return CallRouteeGet<List<ReportResult>>(_baseUrl, "reports/my/volPrice/perMccMnc", query);
        }

        /// <summary>
        /// View Volume/Price summary analytics for a campaign
        /// </summary>
        /// <param name="offset">DateTimeOffset</param>
        /// <param name="campaignId">CampaignId</param>
        /// <returns>RouteResponse with list of ReportResult as Response object in it</returns>
        public RouteResponse<List<ReportResult>> ViewVolumePriceAnalyticsForACampaign(DateTimeOffset offset, string campaignId)
        {
            Dictionary<string, string> query = new Dictionary<string, string>();

            query.Add("offset", JsonConvert.SerializeObject(offset));
            query.Add("campaignId", campaignId);
            return CallRouteeGet<List<ReportResult>>(_baseUrl, "reports/my/volPrice/perCampaign", query);
        }

        /// <summary>
        /// View Time summary analytics for a range of messages
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>RouteResponse with list of SmsLatencyCount as Response object in it</returns>
        public RouteResponse<SmsLatencyCount> ViewTimeSummaryAnalytics(DateTime startDate, DateTime endDate)
        {
            Dictionary<string, string> query = new Dictionary<string, string>();

            query.Add("startDate", JsonConvert.SerializeObject(startDate));
            query.Add("endDate", JsonConvert.SerializeObject(endDate));
            return CallRouteeGet<SmsLatencyCount>(_baseUrl, "reports/my/latency", query);
        }

        /// <summary>
        /// View Time summary analytics for a country
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="countryCode"></param>
        /// <returns>RouteResponse with list of SmsLatencyCount as Response object in it</returns>
        public RouteResponse<SmsLatencyCount> ViewTimeSummaryAnalyticsForACountry(DateTime startDate, DateTime endDate, string countryCode)
        {
            Dictionary<string, string> query = new Dictionary<string, string>();

            query.Add("startDate", JsonConvert.SerializeObject(startDate));
            query.Add("endDate", JsonConvert.SerializeObject(endDate));
            query.Add("countryCode", countryCode);
            return CallRouteeGet<SmsLatencyCount>(_baseUrl, "reports/my/latency/perCountry", query);
        }

        /// <summary>
        /// View Time summary analytics for a country and a network
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="mcc"></param>
        /// <param name="mnc"></param>
        /// <returns>RouteResponse with list of SmsLatencyCount as Response object in it</returns>
        public RouteResponse<SmsLatencyCount> ViewTimeSummaryAnalyticsForACountryAndNetwork(DateTime startDate, DateTime endDate, string mcc, string mnc)
        {
            Dictionary<string, string> query = new Dictionary<string, string>();

            query.Add("startDate", JsonConvert.SerializeObject(startDate));
            query.Add("endDate", JsonConvert.SerializeObject(endDate));
            query.Add("mcc", mcc);
            query.Add("mnc", mnc);
            return CallRouteeGet<SmsLatencyCount>(_baseUrl, "reports/my/latency/perMccMnc", query);
        }

        /// <summary>
        /// View Time summary analytics for a campaign
        /// </summary>
        /// <param name="campaignId">CampaignId</param>
        /// <returns>RouteResponse with list of SmsLatencyCount as Response object in it</returns>
        public RouteResponse<SmsLatencyCount> ViewTimeSummaryAnalyticsForACampaign(string campaignId)
        {
            Dictionary<string, string> query = new Dictionary<string, string>();

            
            query.Add("campaignId", campaignId);
            return CallRouteeGet<SmsLatencyCount>(_baseUrl, "reports/my/latency/perCampaign", query);
        }
        #endregion

        #region Contacts

        /// <summary>
        /// Create a new contact
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>RouteResponse with ContactResponse as Response object in it</returns>
        public RouteResponse<ContactResponse> CreateContact(Contact contact)
        {
            return CallRoutee<ContactResponse, Contact>(contact, _baseUrl, "contacts/my");
        }


        /// <summary>
        /// Delete multiple contacts
        /// </summary>
        /// <param name="contactIds">ContactIds to be deleted</param>
        /// <returns>RouteResponse with list of contact ids deleted as Response object in it</returns>
        public RouteResponse<List<string>> DeleteMultipleContacts(List<string> contactIds )
        {
            return CallRoutee<List<string>, List<string>>(contactIds, _baseUrl, "contacts/my",httpMethod:Method.DELETE);
        }

        /// <summary>
        /// Retrieve all contacts
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns>RouteResponse with ContactList Response object in it</returns>
        public RouteResponse<ContactList> RetrieveAllContacts(int page=0,int size=10)
        {
            Dictionary<string,string> queryparameters=new Dictionary<string, string>();
            queryparameters.Add("page",page.ToString());
            queryparameters.Add("size", size.ToString());
            return CallRouteeGet<ContactList>(_baseUrl, "contacts/my", queryparameters);
        }

        /// <summary>
        /// Retrieve details about a contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>RouteResponse with ContactResponse Response object in it</returns>
        public RouteResponse<ContactResponse> RetrieveContactDetails(string contactId)
        {
            return CallRouteeGet<ContactResponse>(_baseUrl, "contacts/my/"+contactId);
        }

        /// <summary>
        /// Update contact details
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="contact"></param>
        /// <returns>RouteResponse with ContactResponse Response object in it</returns>
        public RouteResponse<ContactResponse> UpdateContactDetails(string contactId, Contact contact)
        {
            return CallRoutee<ContactResponse, Contact>(contact, _baseUrl, "contacts/my/"+contactId,httpMethod:Method.PUT);
        }

        /// <summary>
        /// Delete a contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>RouteResponse with ContactResponse Response object in it</returns>
        public RouteResponse<ContactResponse> DeleteContact(string contactId)
        {
            return CallRoutee<ContactResponse, object>(null, _baseUrl, "contacts/my/" + contactId, httpMethod: Method.DELETE);
        }

        /// <summary>
        /// Check if contact exist
        /// </summary>
        /// <param name="contactNumber"></param>
        /// <returns>HttpStatusCode</returns>
        public HttpStatusCode CheckIfContactExist(string contactNumber)
        {
            Dictionary<string,string> queryparams=new Dictionary<string, string>();
            queryparams.Add("value", contactNumber);
            return CallRoutee<object, object>(null, _baseUrl, "contacts/my/mobile",queryparams, httpMethod: Method.HEAD).StatusCode;
        }

        /// <summary>
        /// Add contacts to blacklist
        /// </summary>
        /// <param name="service"></param>
        /// <param name="contactIds"></param>
        /// <returns>RouteResponse with UpdatedNumber Response object in it</returns>
        public RouteResponse<UpdatedNumber> AddContactsToBlackList(string service, List<string> contactIds)
        {
            var ids = (from contactId in contactIds select new {id = contactId}).ToList();
            return CallRoutee<UpdatedNumber, object>(ids, _baseUrl, "contacts/my/blacklist/" + service);
        }

        /// <summary>
        /// Returns all the contacts which are blacklisted for the given service.
        /// </summary>
        /// <param name="service"></param>
        /// <returns>RouteResponse with List of ContactResponse Response object in it</returns>
        public RouteResponse<List<ContactResponse>> GetBlackListedContactForAService(string service)
        {
            return CallRouteeGet<List<ContactResponse>>(_baseUrl, "contacts/my/blacklist/" + service);
        }

        /// <summary>
        /// Remove a group of contacts from the blacklist
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="groups"></param>
        /// <returns>RouteResponse with List of UpdatedNumber Response object in it</returns>
        public RouteResponse<List<UpdatedNumber>> RemoveContactsFromBlackList(string serviceName, List<string> groups)
        {
            return CallRoutee<List<UpdatedNumber>, object>(groups, _baseUrl, "contacts/my/blacklist/" + serviceName+"/groups",httpMethod:Method.DELETE);
        }

        /// <summary>
        /// Retrieve the account’s labels both default and custom.
        /// </summary>
        /// <returns>RouteResponse with dictionary of label name as key and type as value Response object in it</returns>
        public RouteResponse<Dictionary<string,string>> RetrieveAccountContactLables()
        {
            return CallRouteeGet<Dictionary<string, string>>(_baseUrl, "contacts/labels/my");
        }

        /// <summary>
        /// Creates extra contact labels for the specified account. 
        /// </summary>
        /// <param name="labels">List of contact labels</param>
        /// <returns>RouteResponse with list of ContactLabel Response object in it</returns>
        public RouteResponse<List<ContactLabel>> CreateLables(List<ContactLabel> labels )
        {
            return CallRoutee<List<ContactLabel>, List<ContactLabel>>(labels,_baseUrl, "contacts/labels/my");
        }

        /// <summary>
        /// Retrieve the specified account’s groups of contacts.
        /// </summary>
        /// <returns>RouteResponse with list of AccountGroupName Response object in it</returns>
        public RouteResponse<List<AccountGroupName>> RetrieveAccountGroups()
        {
            return CallRouteeGet<List<AccountGroupName>>(_baseUrl, "groups/my");
        }

        /// <summary>
        /// Returns one of the groups that the account has created with the number of contacts in it.
        /// </summary>
        /// <param name="groupName">Group name</param>
        /// <returns>RouteResponse with list of AccountGroupSize Response object in it</returns>
        public RouteResponse<AccountGroupSize> RetrieveAccountGroupByName(string groupName)
        {
            return CallRouteeGet<AccountGroupSize>(_baseUrl, "groups/my/"+ groupName);
        }

        /// <summary>
        /// Retrieve a page of the specified account’s groups of contacts.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns>RouteResponse with list of PagedAccountGroup Response object in it</returns>
        public RouteResponse<PagedAccountGroup> RetrieveAccountGroupInPageFormat(int page=0, int size=10)
        {
            Dictionary<string,string> queryParameters=new Dictionary<string, string>();
            queryParameters.Add("page",page.ToString());
            queryParameters.Add("size",size.ToString());
            return CallRouteeGet<PagedAccountGroup>(_baseUrl, "groups/my/page",queryParameters);
        }

        /// <summary>
        /// Creates a new group. The group can be either created empty or contacts can be added to it during the creation. The contacts can be added by using filters.
        /// </summary>
        /// <param name="accountGroup"></param>
        /// <returns>RouteResponse with AccountGroupSize Response object in it</returns>
        public RouteResponse<AccountGroupSize> CreateNewGroup(AccountGroup accountGroup)
        {
            return CallRoutee<AccountGroupSize, AccountGroup>(accountGroup,_baseUrl, "groups/my");
        }

        /// <summary>
        /// Deletes groups from the specified account.
        /// </summary>
        /// <param name="groupnames"></param>
        /// <param name="deleteContacts"></param>
        /// <returns>RouteResponse with list of DeletedContacts Response object in it</returns>
        public RouteResponse<List<DeletedContacts>> DeleteGroups(List<string> groupnames, bool deleteContacts=false)
        {
            Dictionary<string,string> queryparams=new Dictionary<string, string>();
            queryparams.Add("deleteContacts", deleteContacts.ToString().ToLower());
            return CallRoutee<List<DeletedContacts>, List<string>>(groupnames, _baseUrl, "groups/my", queryparams,
                Method.DELETE);
        }

        /// <summary>
        /// Creates a new group as a merged result of multiple groups of contacts. Duplicate contacts will be added once in the new group. An extra group tag of the new merged group is added in every associated contact.
        /// </summary>
        /// <param name="name">The name of the group to be created.</param>
        /// <param name="groups">The names of the groups that will be merged.</param>
        /// <returns>RouteResponse with AccountGroupSize Response object in it</returns>
        public RouteResponse<AccountGroupSize> MergeMultipleGroups(string name,List<string> groups )
        {
            var group = new {name = name, groups = groups};
            return CallRoutee<AccountGroupSize, object>(group, _baseUrl, "groups/my/merge");
        }

        /// <summary>
        /// Creates a new group from the difference of contacts between the provided groups.
        /// </summary>
        /// <param name="name">The name of the group to be created.</param>
        /// <param name="groups">The names of the groups used to create the new group.</param>
        /// <returns>RouteResponse with AccountGroupSize Response object in it</returns>
        public RouteResponse<AccountGroupSize> CreateGroupFromDifferences(string name, List<string> groups)
        {
            var group = new { name = name, groups = groups };
            return CallRoutee<AccountGroupSize, object>(group, _baseUrl, "groups/my/difference");
        }

        /// <summary>
        /// View the contacts that a group is consisted of.
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="sort"></param>
        /// <returns>RouteResponse with ContactList Response object in it</returns>
        public RouteResponse<ContactList> GetContactsOfSpecificGroup(string groupname, int page=0,int size=10, string sort=null)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("page",page.ToString());
            queryParams.Add("size", size.ToString());
            if (!string.IsNullOrEmpty(sort))
            {
                queryParams.Add("sort", sort);
            }
            return CallRouteeGet<ContactList>(_baseUrl, "groups/my/"+groupname+"/contacts",queryParams);
        }

        /// <summary>
        /// Deletes the contacts that match the provided ids from the specified group.
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="contacts"></param>
        /// <returns>RouteResponse with AccountGroupSize Response object in it</returns>
        public RouteResponse<AccountGroupSize> DeleteContactsOfSpecificGroup(string groupname, List<string> contacts)
        {
            return CallRoutee<AccountGroupSize, List<string>>(contacts,_baseUrl, "groups/my/" + groupname + "/contacts",httpMethod:Method.DELETE);
        }

        /// <summary>
        /// Update one of your groups by adding existing contacts to it
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="contacts"></param>
        /// <returns>RouteResponse with AccountGroupSize Response object in it</returns>
        public RouteResponse<AccountGroupSize> AddContactsToSpecificGroup(string groupname, List<string> contacts)
        {
            return CallRoutee<AccountGroupSize, List<string>>(contacts, _baseUrl, "groups/my/" + groupname + "/contacts");
        }

        /// <summary>
        /// Extract existing contacts from a service’s blacklist.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="contactIds"></param>
        /// <returns>RouteResponse with UpdatedNumber Response object in it</returns>
        public RouteResponse<UpdatedNumber> RemoveContactsFromServiceBlackList(string service, List<string> contactIds)
        {
            var contacts = (from con in contactIds select new {id = con}).ToList();
            return CallRoutee<UpdatedNumber, object>(contacts, _baseUrl, "contacts/my/blacklist/" + service,httpMethod:Method.DELETE);
        }


        #endregion

        #region Routee 

        private RouteResponse<T> CallRouteeFormUrlEncoded<T>(string baseUrl, string resourceUrl, Dictionary<string, string> queryParameters = null, Dictionary<string, string> formEncoded = null)
        {
            RouteResponse<T> response = new RouteResponse<T>();

            //var client = new RestClient(baseUrl);

            var request = new RestRequest(resourceUrl, Method.POST);
            request.RequestFormat = DataFormat.Json;


            // easily add HTTP Headers
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            if (queryParameters != null && queryParameters.Any())
            {
                foreach (var queryParameter in queryParameters)
                {
                    request.AddQueryParameter(queryParameter.Key, queryParameter.Value);
                }

            }

            if (formEncoded != null && formEncoded.Any())
            {
                foreach (var formencode in formEncoded)
                {
                    request.AddParameter(formencode.Key, formencode.Value);
                }

            }

            // execute the request
            IRestResponse restResponse = _client.Execute(request);
            response.StatusCode = restResponse.StatusCode;

            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                response.Response = JsonConvert.DeserializeObject<T>(restResponse.Content);
            }

            else
            {
                response.Error = JsonConvert.DeserializeObject<Error>(restResponse.Content);
            }

            return response;
        }
        private RouteResponse<T> CallRouteeGet<T>(string baseUrl, string resourceUrl, Dictionary<string, string> queryParameters = null)
        {
            RouteResponse<T> response = new RouteResponse<T>();

            //var client = new RestClient(baseUrl);

            var request = new RestRequest(resourceUrl, Method.GET);
            request.JsonSerializer = new RestSharpCustomSerializer() { ContentType = "application/json" };
            request.RequestFormat = DataFormat.Json;


            // easily add HTTP Headers
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddHeader("Content-Type", "application/json");

            if (queryParameters != null && queryParameters.Any())
            {
                foreach (var queryParameter in queryParameters)
                {
                    request.AddQueryParameter(queryParameter.Key, queryParameter.Value);
                }

            }

            // execute the request
            IRestResponse restResponse = _client.Execute(request);
            response.StatusCode = restResponse.StatusCode;

            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                response.Response = JsonConvert.DeserializeObject<T>(restResponse.Content);
            }

            else
            {
                response.Error = JsonConvert.DeserializeObject<Error>(restResponse.Content);
            }

            return response;
        }
        private RouteResponse<T> CallRoutee<T, Q>(Q obj, string baseUrl, string resourceUrl, Dictionary<string, string> queryParameters = null, Method httpMethod = Method.POST)
        {
            RouteResponse<T> response = new RouteResponse<T>();

            //var client = new RestClient(baseUrl);

            var request = new RestRequest(resourceUrl, httpMethod);
            request.JsonSerializer = new RestSharpCustomSerializer() {ContentType = "application/json"};
            request.RequestFormat = DataFormat.Json;
            if (queryParameters != null && queryParameters.Any())
            {
                foreach (var queryParameter in queryParameters)
                {
                    request.AddQueryParameter(queryParameter.Key, queryParameter.Value);
                }

            }
            
            // easily add HTTP Headers
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddHeader("Content-Type", "application/json");
            if (obj != null)
            {
                request.AddBody(obj);
            }

            // execute the request
            IRestResponse restResponse = _client.Execute(request);
            response.StatusCode = restResponse.StatusCode;

            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                response.Response = JsonConvert.DeserializeObject<T>(restResponse.Content);
            }

            else
            {
                response.Error = JsonConvert.DeserializeObject<Error>(restResponse.Content);
            }

            return response;
        }

        #endregion
    }
}
