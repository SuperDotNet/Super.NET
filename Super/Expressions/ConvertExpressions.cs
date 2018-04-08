using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Model.Specifications;
using Super.Runtime;
using System;
using System.Linq.Expressions;

namespace Super.Expressions
{
	sealed class ConvertExpressions : ReferenceStore<Type, IAlteration<Expression>>
	{
		public static ConvertExpressions Default { get; } = new ConvertExpressions();

		ConvertExpressions() : base(ConvertExpression.Default.Get) {}
	}

	sealed class ConvertExpression : ISource<Type, IAlteration<Expression>>
	{
		public static ConvertExpression Default { get; } = new ConvertExpression();

		ConvertExpression() :
			this(ShouldConvertExpressions.Default.Out(x => x.Select(InstanceTypeCoercer<Expression>.Default)).ToDelegate(),
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
			var result        = alteration.Or(specification).ToAlteration();
			return result;
		}
	}

	sealed class ConvertAlterations : ReferenceStore<Type, IAlteration<Expression>>
	{
		public static ConvertAlterations Default { get; } = new ConvertAlterations();

		ConvertAlterations() : base(x => new ConvertAlteration(x)) {}
	}
}