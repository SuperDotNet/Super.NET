using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using System;

namespace Super.Runtime.Execution
{
	class Contextual<T> : DelegatedSelection<object, T>
	{
		public Contextual(Func<T> source) : this(source.ToInstance()) {}

		public Contextual(ISource<T> source)
			: this(Tables<object, T>.Default.Get(source.Any().ToDelegate()), ExecutionContext.Default) {}

		public Contextual(ISelect<object, T> source, ISource<object> parameter) : base(source, parameter) {}
	}
}