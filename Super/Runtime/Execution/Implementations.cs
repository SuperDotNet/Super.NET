using System;
using Super.Model.Commands;

namespace Super.Runtime.Execution
{
	static class Implementations
	{
		public static IAssign<object, IDisposable> Resources { get; } = AssociatedResources.Default.ToAssignment();
	}
}