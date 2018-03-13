using System;
using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Expressions
{
	sealed class ShouldConvertExpressions : ReferenceStore<Type, ISpecification<Type>>
	{
		public static ShouldConvertExpressions Default { get; } = new ShouldConvertExpressions();

		ShouldConvertExpressions() : base(ShouldConvertExpression.Default.Get) {}
	}
}