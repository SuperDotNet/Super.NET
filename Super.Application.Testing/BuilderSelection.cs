using AutoFixture;
using AutoFixture.Kernel;
using Super.Model.Selection;
using System;

namespace Super.Application.Testing
{
	public class BuilderSelection<T> : ISpecimenBuilderTransformation where T : ISpecimenBuilder
	{
		readonly Func<T, ISpecimenBuilderNode> _delegate;

		protected BuilderSelection(Func<T, ISpecimenBuilder> @delegate)
			: this(@delegate.ToSelect().Out(New<CustomizationNode>.Default)) {}

		protected BuilderSelection(ISelect<T, ISpecimenBuilderNode> select) : this(select.ToDelegate()) {}

		BuilderSelection(Func<T, ISpecimenBuilderNode> @delegate) => _delegate = @delegate;

		public ISpecimenBuilderNode Transform(ISpecimenBuilder builder) => builder.AsTo(_delegate);
	}
}