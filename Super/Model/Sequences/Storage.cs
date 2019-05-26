using Super.Model.Commands;
using Super.Model.Selection;

namespace Super.Model.Sequences
{
	public class Storage<T> : Select<uint, Store<T>>, IStorage<T>
	{
		readonly ICommand<T[]> _return;

		public Storage(IStores<T> stores, ICommand<T[]> @return) : base(stores.Get) => _return = @return;

		public void Execute(T[] parameter)
		{
			_return.Execute(parameter);
		}
	}
}