using Super.Model.Selection;

namespace Super.Text
{
	public interface IMessage<in T> : ISelect<T, string> {}
}