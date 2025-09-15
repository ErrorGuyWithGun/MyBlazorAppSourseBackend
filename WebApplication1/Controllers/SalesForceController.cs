using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using WebApplication1.Models.Salesforce;

[ApiController]
[Route("api/[controller]")]
public class SalesforceController : ControllerBase
{
    private readonly SalesforceService _salesforceService;

    public SalesforceController(SalesforceService salesforceService)
    {
        _salesforceService = salesforceService;
    }

    [HttpGet("GetAccounts")]
    public async Task<IActionResult> GetAccounts()
    {
            var accounts = await _salesforceService.GetAccountsAsync();
            return Ok(accounts);
    }

    [HttpPost("CreateAccount")]
    public async Task<IActionResult> CreateAccount([FromBody] SalesforceAccount account)
    {
            var createdAccount = await _salesforceService.CreateAccountAsync(account);
            return Ok(createdAccount);
    }
}