using Polly;
using Serilog;
using Super.Model.Commands;
using System;

namespace Super.Diagnostics
{
	// ReSharper disable NotAccessedField.Local

	public class LogRetryException : ICommand<(Exception, TimeSpan)>
	{
		readonly ILogger _logger;

		public LogRetryException(ILogger logger) => _logger = logger;

		public void Execute((Exception, TimeSpan) parameter)
		{
			throw new NotImplementedException();
		}
	}

	public class LogRetryException<T> : ICommand<(DelegateResult<T>, TimeSpan, Context)>
	{
		readonly ILogger _logger;

		public LogRetryException(ILogger logger) => _logger = logger;

		public void Execute((DelegateResult<T>, TimeSpan, Context) parameter)
		{
			throw new NotImplementedException();
		}
	}
}