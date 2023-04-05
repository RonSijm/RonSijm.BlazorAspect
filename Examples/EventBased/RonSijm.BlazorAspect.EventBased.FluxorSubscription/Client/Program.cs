namespace RonSijm.BlazorAspect.EventBased.FluxorSubscription.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.WhenAssignableFrom(typeof(IFluxorComponent)).UseAspect(component => FluxorComponentSubscriber.Subscribe(component));

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddFluxor(o => o
            .ScanAssemblies(typeof(Program).Assembly));

        builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        await builder.Build().RunAsync();
    }
}