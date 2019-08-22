using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Model.Commands;
using System;

namespace Super.Application.Services
{
	public sealed class DefaultServiceConfiguration : ICommand<IServiceCollection>
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

	public sealed class ApplicationConfiguration<T> : ICommand<(IApplicationBuilder Builder, IWebHostEnvironment Environment)> where T : IComponent
	{
		public static ApplicationConfiguration<T> Default { get; } = new ApplicationConfiguration<T>();

		ApplicationConfiguration() : this(EndpointConfiguration<T>.Default.Execute) {}

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

	sealed class EndpointConfiguration<T> : ICommand<IEndpointRouteBuilder> where T : IComponent
	{
		public static EndpointConfiguration<T> Default { get; } = new EndpointConfiguration<T>();

		EndpointConfiguration() : this("div#application", "/_Host") {}

		readonly string _selector, _fallback;

		public EndpointConfiguration(string selector, string fallback)
		{
			_selector = selector;
			_fallback = fallback;
		}

		public void Execute(IEndpointRouteBuilder parameter)
		{
			parameter.MapBlazorHub<T>(_selector)
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
}
