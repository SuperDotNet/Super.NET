using System;
using System.Collections.Generic;
using System.Reactive;
using JetBrains.Annotations;
using Super.ExtensionMethods;

namespace Super.Runtime.Execution {
	sealed class Contexts : IContexts
	{
		readonly Stack<object> _context;

		[UsedImplicitly]
		public Contexts() : this(new Context("Root Execution Context")) {}

		public Contexts(object root) : this(new Stack<object>(root.Yield())) {}

		public Contexts(Stack<object> context) => _context = context;

		public object Get() => _context.Peek();

		public void Execute(object parameter)
		{
			_context.Push(parameter);
		}

		public void Execute(Unit parameter)
		{
			switch (_context.Count)
			{
				case 1:
					throw new InvalidOperationException("An attempt was made to dispose of the root execution context, which is not allowed.");
			}
			_context.Pop();
		}
	}
}