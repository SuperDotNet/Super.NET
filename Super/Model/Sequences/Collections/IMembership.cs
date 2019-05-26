using Super.Model.Commands;

namespace Super.Model.Sequences.Collections
{
	public interface IMembership<in T>
	{
		ICommand<T> Add { get; }

		ICommand<T> Remove { get; }
	}
}