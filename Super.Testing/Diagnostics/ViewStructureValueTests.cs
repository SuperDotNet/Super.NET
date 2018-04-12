using Xunit;

namespace Super.Testing.Diagnostics
{
	public sealed class ViewStructureValueTests
	{
		[Fact]
		void Verify()
		{
			/*var configuration = new ConsoleConfiguration(Provider.Default);

			using (var logger = new LoggerConfiguration().To(new ViewAwareSinkDecoration(configuration))
			                                             .CreateLogger())
			{
				logger.Load<Template>()
				      .Execute(AppDomain.CurrentDomain);

				Console.ReadKey();
			}*/
		}

		/*sealed class Template : LogMessage<AppDomain>
		{
			public Template(ILogger logger) : base(logger, "Hello World: {@AppDomain:F}") {}
		}*/
	}
}