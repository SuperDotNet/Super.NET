using Super.Model.Commands;
using Super.Model.Results;

namespace Super.Runtime.Execution
{
	public interface ICounter : IResult<int>, ICommand {}
}