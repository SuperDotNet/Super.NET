using System;
using Serilog;

namespace Super.Diagnostics
{
	public class SeqConfiguration : ILoggingConfiguration
	{
		readonly Uri _uri;

		public SeqConfiguration(Uri uri) => _uri = uri;

		public LoggerConfiguration Get(LoggerConfiguration parameter) => parameter.WriteTo.Seq(_uri.ToString());
	}
}