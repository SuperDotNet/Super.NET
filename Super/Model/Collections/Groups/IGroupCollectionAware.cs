using Super.Model.Commands;

namespace Super.Model.Collections.Groups
{
	public interface IGroupCollectionAware<T> : ICommand<IGroupCollection<T>> {}
}