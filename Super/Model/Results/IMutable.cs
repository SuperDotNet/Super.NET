using Super.Model.Commands;

namespace Super.Model.Results
{
	public interface IMutable<T> : IResult<T>, ICommand<T> {}
}