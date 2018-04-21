using AutoFixture;
using AutoFixture.Kernel;
using JetBrains.Annotations;
using Super.Model.Sources;
using System;
using System.Collections.Generic;

namespace Super.Application.Host.xUnit
{
	public sealed class AutoDataModestAttribute : AutoDataAttribute
	{
		public AutoDataModestAttribute() : base(OptionalParameterAlteration.Default) {}
	}

	public class AutoDataAttribute : AutoFixture.Xunit2.AutoDataAttribute
	{
		public AutoDataAttribute() : this(Fixtures.Default) {}

		protected AutoDataAttribute(params ISpecimenBuilderTransformation[] transformations)
			: this(DefaultCustomization.Default, transformations) {}

		protected AutoDataAttribute(ICustomization customization, params ISpecimenBuilderTransformation[] transformations)
			: this(transformations, customization) {}

		protected AutoDataAttribute(IEnumerable<ISpecimenBuilderTransformation> transformations, ICustomization customization)
			: this(new Fixtures(new EngineParts(transformations.Fixed()), customization)) {}

		protected AutoDataAttribute(ISource<IFixture> source) : base(source.ToDelegate()) {}

		[UsedImplicitly]
		protected AutoDataAttribute(Func<IFixture> fixture) : base(fixture) {}
	}
}