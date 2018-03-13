using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using JetBrains.Annotations;

namespace Super.Testing.Framework
{
	public class AutoDataAttribute : AutoFixture.Xunit2.AutoDataAttribute
	{
		readonly static Func<IFixture> DefaultFixtureFactory = Fixtures<AutoConfiguredMoqCustomization>.Default.Get;

		public AutoDataAttribute() : this(DefaultFixtureFactory) {}

		[UsedImplicitly]
		protected AutoDataAttribute(Func<IFixture> fixture) : base(fixture) {}
	}
}