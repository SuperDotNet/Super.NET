using System;
using JetBrains.Annotations;
using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;

namespace Super.Runtime.Execution {
	sealed class ChildContexts : ISelect<string, IDisposable>, IActivateMarker<IContexts>
	{
		readonly Func<string, object> _context;
		readonly IContexts            _assign;

		[UsedImplicitly]
		public ChildContexts(IContexts assign) : this(I<Context>.Default.From, assign) {}

		public ChildContexts(Func<string, object> context, IContexts assign)
		{
			_context = context;
			_assign  = assign;
		}

		public IDisposable Get(string parameter)
		{
			var child = _context(parameter);
			_assign.Execute(child);
			var result = new DelegatedDisposable(_assign.Execute);
			return result;
		}
	}
}