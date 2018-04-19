using AutoFixture.Kernel;
using Super.Model.Selection;
using System;

namespace Super.Application.Testing
{
	public class BuilderSelection<T> : Select<T, ISpecimenBuilder>, ISpecimenBuilderTransformation
		where T : ISpecimenBuilder
	{
		readonly Func<T, ISpecimenBuilderNode> _delegate;

		protected BuilderSelection(Func<T, ISpecimenBuilderNode> @delegate) : base(@delegate) => _delegate = @delegate;

		public ISpecimenBuilderNode Transform(ISpecimenBuilder builder) => builder.AsTo(_delegate);
	}
}