using System;
using System.Linq.Expressions;
using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;

namespace Super.Expressions
{
	sealed class ConvertExpression : ReferenceStore<Type, IAlteration<Expression>>
	{
		public static ConvertExpression Default { get; } = new ConvertExpression();

		ConvertExpression() : base(x => ConvertAlterations.Default.Get(x)
		                                                  .Out(ShouldConvertExpressions.Default.Get(x))
		                                                  .ToAlteration()) {}
	}
}