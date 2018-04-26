using FluentAssertions;
using Serilog;
using Super.Diagnostics.Logging;
using Xunit;

namespace Super.Testing.Application.Runtime.Execution
{
	public sealed class ObservedContextTests
	{
		readonly Log<ObservedContextTests> _log;
		readonly ILogger            _observed;

		public ObservedContextTests() : this(Log<ObservedContextTests>.Default) {}

		ObservedContextTests(Log<ObservedContextTests> log) : this(log, log.Get()) {}

		ObservedContextTests(Log<ObservedContextTests> log, ILogger observed)
		{
			_log      = log;
			_observed = observed;
		}

		[Fact]
		void VerifyLog()
		{
			_log.Get().Should().BeSameAs(_observed);
		}
	}
}