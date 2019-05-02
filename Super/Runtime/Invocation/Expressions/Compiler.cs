using System.Linq.Expressions;
using Super.Model.Selection;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class Compiler<T> : Select<Expression<T>, T>
	{
		public static Compiler<T> Default { get; } = new Compiler<T>();

		Compiler() : base(x => x.Compile()) {}
	}
}