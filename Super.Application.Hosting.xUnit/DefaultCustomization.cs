﻿using AutoFixture;
using AutoFixture.AutoMoq;

namespace Super.Application.Hosting.xUnit
{
	sealed class DefaultCustomization : CompositeCustomization
	{
		public static DefaultCustomization Default { get; } = new DefaultCustomization();

		DefaultCustomization() : base(ManualPropertyTypesCustomization.Default,
		                              SingletonCustomization.Default,
		                              new InsertCustomization(EpochSpecimen.Default),
		                              new AutoMoqCustomization {ConfigureMembers = true}) {}
	}
}