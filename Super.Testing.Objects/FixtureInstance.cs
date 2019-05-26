using AutoFixture;
using Super.Model.Results;

namespace Super.Testing.Objects
{
	sealed class FixtureInstance : Instance<IFixture>
	{
		public static FixtureInstance Default { get; } = new FixtureInstance();

		FixtureInstance() : base(new Fixture()) {}
	}
}