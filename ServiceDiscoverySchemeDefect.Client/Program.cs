using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Press ENTER to begin.");
Console.ReadLine();

var url = "https+http://myservice/weatherforecast";

var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Services:myservice:http:0"] = $"localhost:5083"
    })
    .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddHttpClient()
    .AddServiceDiscovery()
    .ConfigureHttpClientDefaults(http =>
    {
        http.AddServiceDiscovery();
    })
    .BuildServiceProvider();

var httpClient = serviceProvider
    .GetRequiredService<IHttpClientFactory>()
    .CreateClient();

var response = await httpClient.GetAsync(url);

Console.WriteLine($"Response: {response.StatusCode}");
Console.ReadLine();