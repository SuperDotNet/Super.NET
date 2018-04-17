using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Sources;
using System;

namespace Super.Runtime.Execution
{
	class Contextual<T> : DelegatedSelection<object, T>
	{
		public Contextual(Func<T> source) : this(source.ToSource()) {}

		public Contextual(ISource<T> source) : this(source.Allow().ToStore(), ExecutionContext.Default) {}

		public Contextual(ISelect<object, T> source, ISource<object> parameter) : base(source, parameter) {}
	}
}