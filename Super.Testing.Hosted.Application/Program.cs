using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Model.Commands;
using Super.Testing.Hosted.Application.Data;
using System;

namespace Super.Testing.Hosted.Application
{
	sealed class ServiceConfiguration : ICommand<IServiceCollection>
	{
		public static ServiceConfiguration Default { get; } = new ServiceConfiguration();

		ServiceConfiguration() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddSingleton<WeatherForecastService>();
		}
	}

	sealed class DefaultServiceConfiguration : ICommand<IServiceCollection>
	{
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddRazorPages()
			         .ThenWith(parameter)
			         .AddServerSideBlazor();
		}
	}

	sealed class ApplicationConfiguration : ICommand<(IApplicationBuilder Builder, IWebHostEnvironment Environment)>
	{
		public static ApplicationConfiguration Default { get; } = new ApplicationConfiguration();

		ApplicationConfiguration() : this(EndpointConfiguration.Default.Execute) {}

		readonly Action<IEndpointRouteBuilder> _endpoints;
		readonly string                        _handler;

		public ApplicationConfiguration(Action<IEndpointRouteBuilder> endpoints, string handler = "/Home/Error")
		{
			_endpoints = endpoints;
			_handler   = handler;
		}

		public void Execute((IApplicationBuilder Builder, IWebHostEnvironment Environment) parameter)
		{
			var (builder, environment) = parameter;
			var handle = environment.IsDevelopment()
				             ? builder.UseDeveloperExceptionPage()
				             : builder.UseExceptionHandler(_handler)
				                      .UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

			handle.UseHttpsRedirection()
			      .UseStaticFiles()
			      .UseRouting()
			      .UseEndpoints(_endpoints);
		}
	}

	sealed class EndpointConfiguration : ICommand<IEndpointRouteBuilder>
	{
		public static EndpointConfiguration Default { get; } = new EndpointConfiguration();

		EndpointConfiguration() : this("/_Host") {}

		readonly string _fallback;

		public EndpointConfiguration(string fallback) => _fallback = fallback;

		public void Execute(IEndpointRouteBuilder parameter)
		{
			parameter.MapBlazorHub()
			         .ThenWith(parameter)
			         .MapFallbackToPage(_fallback);
		}
	}

	public interface IHostedApplication
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		void ConfigureServices(IServiceCollection services);

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		void Configure(IApplicationBuilder app, IWebHostEnvironment env);
	}

	public class HostedApplication : IHostedApplication
	{
		readonly Action<IServiceCollection>                                             _services;
		readonly Action<(IApplicationBuilder Builder, IWebHostEnvironment Environment)> _application;

		public HostedApplication(Action<IServiceCollection> services,
		                         Action<(IApplicationBuilder Builder, IWebHostEnvironment Environment)> application)
		{
			_services    = services;
			_application = application;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			_services(services);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			_application((app, env));
		}
	}

	public sealed class Program : HostedApplication
	{
		static void Main(string[] args)
		{
			Host.CreateDefaultBuilder(args)
			    .ConfigureWebHostDefaults(x => x.UseStartup<Program>())
			    .Build()
			    .Run();
		}

		public Program() : base(ServiceConfiguration.Default.Then(DefaultServiceConfiguration.Default),
		                        ApplicationConfiguration.Default.Execute) {}
	}
}