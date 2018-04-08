using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using System;

namespace Super.Expressions
{
	sealed class ShouldConvertExpression : ISource<Type, ISpecification<Type>>
	{
		public static ShouldConvertExpression Default { get; } = new ShouldConvertExpression();

		ShouldConvertExpression() : this(Types.Void.Not()) {}

		readonly ISpecification<Type> _null;

		public ShouldConvertExpression(ISpecification<Type> @null) => _null = @null;

		public ISpecification<Type> Get(Type parameter) => _null.And(parameter.Not());
	}

	sealed class ShouldConvertExpressions : ReferenceStore<Type, ISpecification<Type>>
	{
		public static ShouldConvertExpressions Default { get; } = new ShouldConvertExpressions();

		ShouldConvertExpressions() : base(ShouldConvertExpression.Default.Get) {}
	}

}