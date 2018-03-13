using System;
using AutoFixture.Kernel;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Testing.Framework
{
	public class EngineParts<T> : DelegatedSource<T, ISpecimenBuilder>, ISpecimenBuilderTransformation
		where T : ISpecimenBuilder
	{
		readonly Func<T, ISpecimenBuilderNode> _delegate;

		protected EngineParts(Func<T, ISpecimenBuilderNode> @delegate) : base(@delegate) => _delegate = @delegate;

		public ISpecimenBuilderNode Transform(ISpecimenBuilder builder) => builder.AsTo(_delegate);
	}
}