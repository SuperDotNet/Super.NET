using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Runtime;
using System;
using System.Linq.Expressions;

namespace Super.Expressions
{
	sealed class ConvertExpression : ReferenceStore<Type, IAlteration<Expression>>
	{
		public static ConvertExpression Default { get; } = new ConvertExpression();

		ConvertExpression() : base(x => // TODO: Clean up.
			                           ConvertAlterations.Default.Get(x).ToDelegate().Or(ShouldConvertExpressions.Default.Get(x).Select(InstanceTypeCoercer<Expression>.Default).ToDelegate(), _ => null)
			                           .ToAlteration()) {}
	}

	sealed class ConvertAlterations : ReferenceStore<Type, IAlteration<Expression>>
	{
		public static ConvertAlterations Default { get; } = new ConvertAlterations();

		ConvertAlterations() : base(x => new ConvertAlteration(x)) {}
	}
}