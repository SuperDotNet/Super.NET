using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Application.Services;
using Super.Model.Commands;
using Super.Sample.Application.Data;

namespace Super.Sample.Application
{
	sealed class Program : HostedApplication
	{
		static void Main(string[] args)
		{
			Host.CreateDefaultBuilder(args)
			    .ConfigureWebHostDefaults(x => x.UseStartup<Program>())
			    .Build()
			    .Run();
		}

		public Program() : base(ServiceConfiguration.Default.Then(DefaultServiceConfiguration.Default),
		                        ApplicationConfiguration<Application>.Default.Execute) {}
	}

	sealed class ServiceConfiguration : Command<IServiceCollection>
	{
		public static ServiceConfiguration Default { get; } = new ServiceConfiguration();

		ServiceConfiguration() : base(x => x.AddSingleton<WeatherForecastService>()) {}
	}
}