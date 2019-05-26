using Super.Model.Results;
using Super.Model.Selection;

namespace Super.Model.Sequences
{
	public interface IArray<T> : IResult<Array<T>> {}

	public interface IArray<in _, T> : ISelect<_, Array<T>> {}
}