using FluentAssertions;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Super.Diagnostics.Logging.Configuration;
using Super.Text.Formatting;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Diagnostics
{
	public sealed class ApplyProjectionsCommandTests
	{
		[Fact]
		void Verify()
		{
			var sink = new TextSink();

			using (var logger = new LoggerConfiguration().To(ProjectionsConfiguration.Default)
			                                             .WriteTo.Sink(sink)
			                                             .CreateLogger())
			{
				logger.Information("Testing: {AppDomain}", AppDomain.CurrentDomain);
				logger.Information("Testing: {AppDomain:F}", AppDomain.CurrentDomain);
				logger.Information("Testing: {AppDomain:I}", AppDomain.CurrentDomain);
			}

			sink.Should().Equal("Testing: AppDomain: testhost", "Testing: testhost", $"Testing: {AppDomain.CurrentDomain.Id}");
		}

		[Fact]
		void VerifyMultiple()
		{
			var sink = new TextSink(Provider.Default);

			using (var logger = new LoggerConfiguration().To(ProjectionsConfiguration.Default)
			                                             .WriteTo.Sink<TextSink>()
			                                             .WriteTo.Sink(new StructureSink())
			                                             .WriteTo.Sink(sink)
			                                             .CreateLogger())
			{
				logger.Information("Testing: {AppDomain}", AppDomain.CurrentDomain);
				logger.Information("Testing: {AppDomain:F}", AppDomain.CurrentDomain);
				logger.Information("Testing: {AppDomain:I}", AppDomain.CurrentDomain);
			}

			sink.Should().Equal("Testing: AppDomain: testhost", "Testing: testhost", $"Testing: {AppDomain.CurrentDomain.Id}");
		}

		[Fact]
		void VerifyMultipleEnabled()
		{
			var sink = new TextSink(Provider.Default);

			using (var logger = new LoggerConfiguration().To(ProjectionsConfiguration.Default)
			                                             .WriteTo.Sink<TextSink>()
			                                             .To(new StructureSink().WithProjections())
			                                             .WriteTo.Sink(sink)
			                                             .CreateLogger())
			{
				logger.Information("Testing: {AppDomain}", AppDomain.CurrentDomain);
				logger.Information("Testing: {AppDomain:F}", AppDomain.CurrentDomain);
				logger.Information("Testing: {AppDomain:I}", AppDomain.CurrentDomain);
			}

			sink.Should().Equal("Testing: AppDomain: testhost", "Testing: testhost", $"Testing: {AppDomain.CurrentDomain.Id}");
		}

		[Fact]
		void VerifyStructure()
		{
			var sink = new StructureSink();
			using (var logger = new LoggerConfiguration().To(ProjectionsConfiguration.Default)
			                                             .To(sink.WithProjections())
			                                             .CreateLogger())
			{
				logger.Information("Testing: {AppDomain}", AppDomain.CurrentDomain);
				logger.Information("Testing: {AppDomain:I}", AppDomain.CurrentDomain);
			}

			var @default = sink.First();
			@default.TypeTag.Should().Be("AppDomain");
			@default.Properties.Should().HaveCount(2);
			@default.Properties[0].Value.To<ScalarValue>().Value.Should().Be(AppDomain.CurrentDomain.FriendlyName);
			@default.Properties[1].Value.To<ScalarValue>().Value.Should().Be(AppDomain.CurrentDomain.Id);

			var identifier = sink.ElementAt(1);
			identifier.TypeTag.Should().Be("AppDomain");
			identifier.Properties.Should().HaveCount(4);
			identifier.Properties[0].Value.To<ScalarValue>().Value.Should().Be(AppDomain.CurrentDomain.FriendlyName);
			identifier.Properties[1].Value.To<ScalarValue>().Value.Should().Be(AppDomain.CurrentDomain.Id);
			identifier.Properties[2].Value.To<ScalarValue>().Value.Should().Be(AppDomain.CurrentDomain.BaseDirectory);
			identifier.Properties[3].Value.To<ScalarValue>().Value.Should().Be(AppDomain.CurrentDomain.RelativeSearchPath);
		}

		sealed class StructureSink : Collection<StructureValue>, ILogEventSink
		{
			public void Emit(LogEvent logEvent)
			{
				logEvent.Properties.Values.OfType<StructureValue>().ForEach(Add);
			}
		}

		sealed class TextSink : Collection<string>, ILogEventSink
		{
			readonly IFormatProvider _services;

			public TextSink() : this(Provider.Default) {}

			public TextSink(IFormatProvider services) => _services = services;

			public void Emit(LogEvent logEvent)
			{
				Add(logEvent.RenderMessage(_services));
			}
		}
	}
}