using Super.Model.Commands;

namespace Super.Model.Collections
{
	public interface IGroupCollectionAware<T> : ICommand<IGroupCollection<T>> {}
}