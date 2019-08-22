using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Super.Application.Services;
using Super.Model.Commands;
using Super.Sample.Application.Data;

namespace Super.Sample.Application
{
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

	sealed class ServiceConfiguration : ICommand<IServiceCollection>
	{
		public static ServiceConfiguration Default { get; } = new ServiceConfiguration();

		ServiceConfiguration() {}

		public void Execute(IServiceCollection parameter)
		{

			parameter.AddSingleton<WeatherForecastService>();
		}
	}
}