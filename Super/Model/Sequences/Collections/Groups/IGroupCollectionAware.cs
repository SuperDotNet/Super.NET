using Super.Model.Commands;

namespace Super.Model.Sequences.Collections.Groups
{
	public interface IGroupCollectionAware<T> : ICommand<IGroupCollection<T>> {}
}