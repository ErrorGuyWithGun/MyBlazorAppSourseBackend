using Newtonsoft.Json;

namespace WebApplication1.Models.Salesforce
{
    public class SalesforceAuthResponse
    {
        public string access_token { get; set; }
        public string instance_url { get; set; }
    }
}
