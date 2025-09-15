using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebApplication1.Models.Salesforce;

namespace WebApplication1.Services
{
    public class SalesforceService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private string _accessToken;
        private string _instanceUrl;

        public SalesforceService(IConfiguration config, 
            IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", _config["Salesforce:ClientId"]),
                new KeyValuePair<string, string>("client_secret", _config["Salesforce:ClientSecret"]),
                new KeyValuePair<string, string>("username", _config["Salesforce:Username"]),
                new KeyValuePair<string, string>("password", _config["Salesforce:Password"])
            });

            var response = await client.PostAsync($"{_config["Salesforce:LoginUrl"]}/services/oauth2/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var authResponse = JsonConvert.DeserializeObject<SalesforceAuthResponse>(responseContent);
            _accessToken = authResponse.access_token;
            _instanceUrl = authResponse.instance_url;

            return _accessToken;
        }

        public async Task<List<SalesforceAccount>> GetAccountsAsync()
        {
            var accessToken = await GetAccessTokenAsync();
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync($"{_instanceUrl}/services/data/{_config["Salesforce:ApiVersion"]}/query?q=SELECT+Id,Name,Phone,Website+FROM+Account");
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<SalesforceGetResult<SalesforceAccount>>(responseContent);
            return result.Records;
        }

        public async Task<SalesforceAccount> CreateAccountAsync(SalesforceAccount account)
        {
            var accessToken = await GetAccessTokenAsync();
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SalesforceAccount accountData = new SalesforceAccount
            {
                Name = account.Name,
                Phone = account.Phone,
                Website = account.Website
            };

            var content = new StringContent(JsonConvert.SerializeObject(accountData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_instanceUrl}/services/data/{_config["Salesforce:ApiVersion"]}/sobjects/Account/", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var createResponse = JsonConvert.DeserializeObject<SalesforceCreateResponse>(responseContent);

            accountData.Id = createResponse.Id;

            return accountData;
        }
    }
}