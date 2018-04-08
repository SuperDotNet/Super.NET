using Super.Model.Selection;

namespace Super.Text
{
	public interface IFormatter<in T> : ISelect<T, string> {}
}