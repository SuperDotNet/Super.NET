using System;
using AutoFixture.Kernel;
using Super.ExtensionMethods;
using Super.Model.Selection;

namespace Super.Testing.Framework
{
	public class EngineParts<T> : Delegated<T, ISpecimenBuilder>, ISpecimenBuilderTransformation
		where T : ISpecimenBuilder
	{
		readonly Func<T, ISpecimenBuilderNode> _delegate;

		protected EngineParts(Func<T, ISpecimenBuilderNode> @delegate) : base(@delegate) => _delegate = @delegate;

		public ISpecimenBuilderNode Transform(ISpecimenBuilder builder) => builder.AsTo(_delegate);
	}
}