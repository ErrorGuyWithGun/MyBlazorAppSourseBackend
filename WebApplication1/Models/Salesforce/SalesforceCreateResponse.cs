namespace WebApplication1.Models.Salesforce
{
    public class SalesforceCreateResponse
    {
        public string Id { get; set; }

        public bool Success { get; set; }

        public List<string> Errors { get; set; }
    }
}
