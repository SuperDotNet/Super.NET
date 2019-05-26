using System.Threading.Tasks;
using Super.Model.Results;
using Super.Model.Selection;

namespace Super.Operations
{
	public interface IOperation<in TIn, TOut> : ISelect<TIn, ValueTask<TOut>> {}

	public interface IOperation<T> : IResult<ValueTask<T>> {}
}