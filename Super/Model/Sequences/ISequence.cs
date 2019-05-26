using Super.Model.Results;

namespace Super.Model.Sequences
{
	public interface ISequence<T> : IResult<Store<T>> {}
}