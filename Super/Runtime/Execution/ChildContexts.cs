using JetBrains.Annotations;
using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime.Execution
{
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
			var context = _context(parameter);
			_assign.Execute(context);
			var result = new DelegatedDisposable(_assign.Execute);
			return result;
		}
	}
}