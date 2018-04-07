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

		ShouldConvertExpression() : this(new EqualitySpecification<Type>(Types.Void).Inverse()) {}

		readonly ISpecification<Type> _null;

		public ShouldConvertExpression(ISpecification<Type> @null) => _null = @null;

		public ISpecification<Type> Get(Type parameter) => _null.And(new EqualitySpecification<Type>(parameter).Inverse());
	}

	sealed class ShouldConvertExpressions : ReferenceStore<Type, ISpecification<Type>>
	{
		public static ShouldConvertExpressions Default { get; } = new ShouldConvertExpressions();

		ShouldConvertExpressions() : base(ShouldConvertExpression.Default.Get) {}
	}

}