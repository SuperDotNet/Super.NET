﻿using System;
using AutoFixture;
using AutoFixture.Kernel;
using Super.Reflection;

namespace Super.Application.Hosting.xUnit
{
	public class BuilderSelection<T> : ISpecimenBuilderTransformation where T : ISpecimenBuilder
	{
		BuilderSelection(Func<T, ISpecimenBuilderNode> @delegate) => _delegate = @delegate;

		readonly Func<T, ISpecimenBuilderNode> _delegate;

		protected BuilderSelection(Func<T, ISpecimenBuilder> @delegate)
			: this(@delegate.ToSelect().Select(I<CustomizationNode>.Default.New).Get) {}

		public ISpecimenBuilderNode Transform(ISpecimenBuilder builder) => builder.AsTo(_delegate);
	}
}