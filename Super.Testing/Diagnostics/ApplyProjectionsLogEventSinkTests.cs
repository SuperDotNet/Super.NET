using FluentAssertions;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Super.Diagnostics;
using Super.Model.Selection;
using Super.Text.Formatting;
using System;
using System.Collections.ObjectModel;
using Xunit;

namespace Super.Testing.Diagnostics
{
	public sealed class ApplyProjectionsLogEventSinkTests
	{
		[Fact]
		void Verify()
		{
			var sink = new TextSink(Provider.Default);

			using (var logger = new LoggerConfiguration().To(ProjectionsConfiguration.Default)
			                                             .WriteTo.Sink(sink)
			                                             .CreateLogger())
			{
				logger.Information("Testing: {AppDomain}", AppDomain.CurrentDomain);
				logger.Information("Testing: {AppDomain:F}", AppDomain.CurrentDomain);
				logger.Information("Testing: {AppDomain:I}", AppDomain.CurrentDomain);
			}

			sink.Should().Equal("Testing: AppDomain: Super.Testing", "Testing: Super.Testing", $"Testing: {AppDomain.CurrentDomain.Id}");
		}

		sealed class TextSink : Collection<string>, ILogEventSink
		{
			readonly IFormatProvider _services;

			public TextSink(IFormatProvider services) => _services = services;

			public void Emit(LogEvent logEvent)
			{
				Add(logEvent.RenderMessage(_services));
			}
		}
	}
}