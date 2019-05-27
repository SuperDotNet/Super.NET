using Super.Model.Selection.Stores;
using System;
using System.Diagnostics;

namespace Super.Diagnostics
{
	sealed class EnhancedExceptions : DecoratedTable<Exception, Exception>
	{
		public static EnhancedExceptions Default { get; } = new EnhancedExceptions();

		EnhancedExceptions() : base(ReferenceTables<Exception, Exception>.Default.Get(x => x.Demystify())) {}
	}
}