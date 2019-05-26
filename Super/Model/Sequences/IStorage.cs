using Super.Model.Commands;

namespace Super.Model.Sequences
{
	public interface IStorage<T> : IStores<T>, ICommand<T[]> {}
}