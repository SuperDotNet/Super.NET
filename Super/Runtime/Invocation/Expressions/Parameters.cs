using System.Linq.Expressions;
using Super.Model.Selection.Stores;
using Super.Reflection.Types;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class Parameters<T> : ReferenceValueStore<string, ParameterExpression>
	{
		public static Parameters<T> Default { get; } = new Parameters<T>();

		Parameters() : base(new Parameter(Type<T>.Instance).Get) {}
	}
}