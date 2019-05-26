using Super.Model.Selection;

namespace Super.Runtime.Objects
{
	public interface IProperty<in T> : ISelect<T, Pair<string, object>> {}
}