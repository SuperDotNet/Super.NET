using Super.Model.Results;

namespace Super.Runtime.Activation
{
	public interface ISingleton<out T> : IResult<T> {}
}