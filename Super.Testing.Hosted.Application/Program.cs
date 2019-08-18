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
	public sealed class Program
	{
		public static void Main(string[] args)
		{
			Host.CreateDefaultBuilder(args)
			    .ConfigureWebHostDefaults(x => x.UseStartup<Program>())
			    .Build()
			    .Run();
		}

		readonly Action<IEndpointRouteBuilder> _endpoints;

		public Program() : this(EndpointConfiguration.Default.Execute) {}

		public Program(Action<IEndpointRouteBuilder> endpoints) => _endpoints = endpoints;

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<WeatherForecastService>()
			        .AddRazorPages()
			        .Then(services)
			        .AddServerSideBlazor();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection()
			   .UseStaticFiles()
			   .UseRouting()
			   .UseEndpoints(_endpoints);
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
				         .Then(parameter)
				         .MapFallbackToPage(_fallback);
			}
		}
	}
}