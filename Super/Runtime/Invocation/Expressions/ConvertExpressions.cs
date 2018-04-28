using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Stores;
using Super.Model.Specifications;
using System;
using System.Linq.Expressions;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class ConvertExpressions : ReferenceValueTable<Type, IAlteration<Expression>>
	{
		public static ConvertExpressions Default { get; } = new ConvertExpressions();

		ConvertExpressions() : base(ConvertExpression.Default.Get) {}
	}

	sealed class ConvertExpression : ISelect<Type, IAlteration<Expression>>
	{
		public static ConvertExpression Default { get; } = new ConvertExpression();

		ConvertExpression() : this(ShouldConvertExpressions.Default.Select(In<Expression>.Type().Out).Get,
		                           ConvertAlterations.Default.Get) {}

		readonly Func<Type, ISpecification<Expression>> _specification;
		readonly Func<Type, IAlteration<Expression>>    _alteration;

		public ConvertExpression(Func<Type, ISpecification<Expression>> specification,
		                         Func<Type, IAlteration<Expression>> alteration)
		{
			_specification = specification;
			_alteration    = alteration;
		}

		public IAlteration<Expression> Get(Type parameter)
		{
			var alteration    = _alteration(parameter);
			var specification = _specification(parameter);
			var result        = alteration.If(specification).ToAlteration();
			return result;
		}
	}

	sealed class ConvertAlterations : ReferenceValueTable<Type, IAlteration<Expression>>
	{
		public static ConvertAlterations Default { get; } = new ConvertAlterations();

		ConvertAlterations() : base(x => new ConvertAlteration(x)) {}
	}
}