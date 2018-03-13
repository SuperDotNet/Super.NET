using Super.Model.Commands;

namespace Super.Model.Collections
{
	public interface IMembership<in T>
	{
		ICommand<T> Add { get; }

		ICommand<T> Remove { get; }
	}
}