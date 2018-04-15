using System;

namespace Super.Environment
{
	sealed class SeqConfiguration : Diagnostics.Logging.Configuration.SeqConfiguration
	{
		public static SeqConfiguration Default { get; } = new SeqConfiguration();

		SeqConfiguration() : base(new Uri("http://localhost:5341")) {}
	}
}