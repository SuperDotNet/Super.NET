using Super.Model.Selection.Stores;
using System;
using System.Diagnostics;

namespace Super.Diagnostics
{
	sealed class EnhancedExceptions : ReferenceValueTable<Exception, Exception>
	{
		public static EnhancedExceptions Default { get; } = new EnhancedExceptions();

		EnhancedExceptions() : base(x => x.Demystify()) {}
	}
}