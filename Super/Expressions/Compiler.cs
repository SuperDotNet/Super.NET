using System.Linq.Expressions;
using Super.Model.Sources;

namespace Super.Expressions
{
	sealed class Compiler<T> : ISource<Expression<T>, T>
	{
		public static Compiler<T> Default { get; } = new Compiler<T>();

		Compiler() {}

		public T Get(Expression<T> parameter) => parameter.Compile();
	}
}