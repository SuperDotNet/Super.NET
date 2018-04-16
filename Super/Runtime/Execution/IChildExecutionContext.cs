using System;
using Super.Model.Selection;

namespace Super.Runtime.Execution {
	public interface IChildExecutionContext : ISelect<string, IDisposable> {}
}