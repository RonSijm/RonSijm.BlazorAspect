// ReSharper disable once ClassNeverInstantiated.Global

namespace RonSijm.BlazorAspect.EventBased.SimpleLogging.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.WhenAssignableFrom(typeof(ILogRendering)).UseAspect(component => LogRenderingAction.Log(component));

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Logging.AddFilter("*", LogLevel.None);
        builder.Logging.AddFilter("RonSijm.BlazorAspect.AspectActivation", LogLevel.Debug);
        var application = builder.Build();

        application.Services.EnableComponentResolveLogging();

        await application.RunAsync();
    }
}