using Microsoft.AspNetCore.Mvc;
using Orleans;
using Sphere.Interfaces;
using Sphere.Shared.Models;

namespace Sphere.Accounts.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IClusterClient _client;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IClusterClient client)
    {
        _logger = logger;
        _client = client;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var accountGrain = _client.GetGrain<IAccount>("test");
        var state = await accountGrain.GetAccountState();
        // await accountGrain.RegisterAccount(new AccountState
        // {
        //     AccountDisplayName = "Test Name",
        //     AccountId = 2,
        //     BirthDate = new DateOnly(1979, 08, 11),
        //     CreatedAt = DateTime.UtcNow,
        //     CreatedWith = CreatedWith.Web,
        //     Email = "test.user@gmail.com",
        //     TimeZone = TimeZoneInfo.Local.ToSerializedString(),
        //     UserName = "@TestUser",
        //     PhoneNumber = "+15555551212",
        // });

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
