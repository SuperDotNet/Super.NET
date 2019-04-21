using AutoFixture;
using AutoFixture.Kernel;
using JetBrains.Annotations;
using Super.Model.Results;
using System;
using System.Collections.Generic;

namespace Super.Application.Hosting.xUnit
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
			: this(new Fixtures(new EngineParts(transformations.Open()), customization)) {}

		protected AutoDataAttribute(IResult<IFixture> result) : base(result.Get) {}

		[UsedImplicitly]
		protected AutoDataAttribute(Func<IFixture> fixture) : base(fixture) {}
	}
}