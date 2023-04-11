# RonSijm.BlazorAspect

[![.NET](https://github.com/RonSijm/RonSijm.BlazorAspect/actions/workflows/build_main.yml/badge.svg?branch=main)](https://github.com/RonSijm/RonSijm.BlazorAspect/actions/workflows/build_main.yml) [![NuGet](https://img.shields.io/nuget/v/RonSijm.BlazorAspect)](https://www.nuget.org/packages/RonSijm.BlazorAspect/) [![codecov](https://codecov.io/gh/RonSijm/RonSijm.BlazorAspect/branch/main/graph/badge.svg?token=PIDRVFD6IW)](https://codecov.io/gh/RonSijm/RonSijm.BlazorAspect)

A C# Blazor library To assign Aspects / Behaviors to a page or component through assigning an interface, attribute, or type any type expression


NuGet: https://www.nuget.org/packages/RonSijm.BlazorAspect/

# What is this library:

This library aims to add Aspect to Blazor pages or components, and allows you to basically delegate OnAfterRender to a standalone class.

Practically this library introduces a hook after `OnParametersSetAsync` and Before `OnAfterRender` that can be used by interface based subscribers.

In usual C# you might want to implement similar behavior through PostSharp or Fody, and handle the AOP side through code-weaving during compile time, but in Blazor those approaches are not always ideal, and it's sometimes preferred to handle things run-time

Or in usual C# you would lean towards Castle DynamicProxy and interceptors - that's not doing to work very well in Blazor

# Youtube Video Demo:  

[![Video of RonSijm.BlazorAspect](https://i.ytimg.com/vi_webp/6xW6xXYBT_Q/maxresdefault.webp)](https://youtu.be/6xW6xXYBT_Q)

# Example Use-Cases

A good example of a use-case is Fluxor.Blazor.

Fluxor.Blazor allows you to "Automagically" subscribes to IState components through inheriting from a [FluxorComponent](https://github.com/mrpmorris/Fluxor/blob/master/Source/Lib/Fluxor.Blazor.Web/Components/FluxorComponent.cs)  
The downside of that, is that you have to inherit all your classes from the FluxorComponent. And as you (should) know - ["favor composition over inheritance"](https://en.wikipedia.org/wiki/Composition_over_inheritance)

This library will allow you to replace the inheritance with composition though interfaces, attributes or type conditions.

# Getting started:

To enable BlazorAspects:  

In your `program.cs` simply start adding aspects with a the line such as:  
`builder.Services.WhenAssignableFrom(typeof(ILogRendering))
                 .UseAspect(component => LogRenderingAction.Log(component));`
	
For example:

````
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.WhenAssignableFrom(typeof(ILogRendering))
                        .UseAspect(component => LogRenderingAction.Log(component));
		// Other stuff
	}
}
````

`WhenAssignableFrom` is expected to be the most common use-case, you can also use `WhenHasAttribute` to use attributes instead of interfaces.  

Then you simply add the line:  
`builder.Services.WhenHasAttribute(typeof(LoggingAttribute))
                 .UseAspect(component => LogRenderingAction.Log(component));`

you can also use your own type condition   :

For example:

`builder.Services.WhenType(x => x.IsAssignableFrom(typeof(ILogRendering)))
                 .UseAspect(component => LogRenderingAction.Log(component));`

or even:  

`builder.Services.WhenType(x => x != null))
                 .UseAspect(component => LogRenderingAction.Log(component));`

# To enable logging:

To enable logging, you can add the following line to your `program.cs`:  

It used the default ILogger, and can be configured as such.

````
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
		
        // I think you have to add this before building
        builder.Logging.AddFilter("RonSijm.BlazorAspect.AspectActivation", LogLevel.Trace);
		
        var application = builder.Build();

        //Add this after building
        application.Services.EnableComponentResolveLogging();

        await application.RunAsync();
    }
````