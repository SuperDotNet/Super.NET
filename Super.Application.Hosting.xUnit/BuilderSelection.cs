using AutoFixture;
using AutoFixture.Kernel;
using Super.Reflection;
using System;

namespace Super.Application.Hosting.xUnit
{
	public class BuilderSelection<T> : ISpecimenBuilderTransformation where T : ISpecimenBuilder
	{
		readonly Func<T, ISpecimenBuilderNode> _delegate;

		protected BuilderSelection(Func<T, ISpecimenBuilder> @delegate)
			: this(@delegate.ToSelect().Select(I<CustomizationNode>.Default.New).Get) {}

		BuilderSelection(Func<T, ISpecimenBuilderNode> @delegate) => _delegate = @delegate;

		public ISpecimenBuilderNode Transform(ISpecimenBuilder builder) => builder.AsTo(_delegate);
	}
}