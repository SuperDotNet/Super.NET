using System.Collections.Generic;
using AutoFixture;
using Super.Model.Results;

namespace Super.Testing.Objects
{
	sealed class Strings : Instance<IEnumerable<string>>
	{
		public static Strings Default { get; } = new Strings();

		Strings() : base(new Fixture().CreateMany<string>()) {}
	}
}