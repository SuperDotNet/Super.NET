using AutoFixture;
using JetBrains.Annotations;
using System;

namespace Super.Application.Testing
{
	public class AutoDataAttribute : AutoFixture.Xunit2.AutoDataAttribute
	{
		readonly static Func<IFixture> DefaultFixtureFactory = Fixtures<DefaultCustomization>.Default.Get;

		public AutoDataAttribute() : this(DefaultFixtureFactory) {}

		[UsedImplicitly]
		protected AutoDataAttribute(Func<IFixture> fixture) : base(fixture) {}
	}
}