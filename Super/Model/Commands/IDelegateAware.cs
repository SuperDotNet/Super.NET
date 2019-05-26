using System;
using Super.Model.Results;

namespace Super.Model.Commands
{
	public interface IDelegateAware<in T> : IResult<Action<T>> {}
}