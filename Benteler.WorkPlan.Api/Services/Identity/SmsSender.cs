using Microsoft.Extensions.Options;

namespace Benteler.WorkPlan.Api.Services.Identity;

public class SmsSender
{
    public Task SendSmsAsync(string number, string message)
    {


        return Task.FromResult(0);
    }
}