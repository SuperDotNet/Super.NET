using Super.Model.Results;

namespace Super.Testing.Objects
{
	sealed class Data : Instance<string[]>
	{
		public static Data Default { get; } = new Data();

		Data() : base(FixtureInstance.Default.Many<string>(10_000)
		                             .Result()
		                             .Get()) {}
	}
}