using System.Collections.Generic;

namespace Super.Model.Sources
{
	public interface IValueSource<in TParameter, out TResult> : ISource<TParameter, TResult>, IEnumerable<TResult> {}
}