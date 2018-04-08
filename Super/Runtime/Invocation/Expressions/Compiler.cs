using System.Linq.Expressions;
using Super.Model.Selection;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class Compiler<T> : ISelect<Expression<T>, T>
	{
		public static Compiler<T> Default { get; } = new Compiler<T>();

		Compiler() {}

		public T Get(Expression<T> parameter) => parameter.Compile();
	}
}