using Super.Runtime;

namespace Super.Model.Selection.Adapters
{
	public interface IAction<in T> : ISelect<T, None> {}
}