using Super.Model.Results;

namespace Super.Runtime
{
	public interface IPair<TKey, TValue> : IResult<Pair<TKey, TValue>> {}
}