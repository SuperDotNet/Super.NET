using Super.Compose;
using Super.Model.Selection.Stores;
using System;
using System.Diagnostics;

namespace Super.Diagnostics
{
	sealed class EnhancedExceptions : DecoratedTable<Exception, Exception>
	{
		public static EnhancedExceptions Default { get; } = new EnhancedExceptions();

		EnhancedExceptions() : base(Start.A.Selection<Exception>().AndOf<Exception>().Into.Table(x => x.Demystify())) {}
	}
}