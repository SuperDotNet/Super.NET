using System;
using AutoFixture;
using JetBrains.Annotations;

namespace Super.Application.Testing
{
	public class AutoDataAttribute : AutoFixture.Xunit2.AutoDataAttribute
	{
		readonly static Func<IFixture> DefaultFixtureFactory = Fixtures<AutoMoqCustomization>.Default.Get;

		public AutoDataAttribute() : this(DefaultFixtureFactory) {}

		[UsedImplicitly]
		protected AutoDataAttribute(Func<IFixture> fixture) : base(fixture) {}
	}
}