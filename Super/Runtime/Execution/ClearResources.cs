using Super.Model.Selection.Stores;

namespace Super.Runtime.Execution
{
	sealed class ClearResources : RemoveCommand<object, Disposables>
	{
		public static ClearResources Default { get; } = new ClearResources();

		ClearResources() : base(AssociatedResources.Default) {}
	}
}