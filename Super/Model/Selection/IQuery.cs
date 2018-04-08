using System.Collections.Generic;

namespace Super.Model.Selection
{
	public interface IQuery<in TParameter, out TResult> : ISelect<TParameter, TResult>, IEnumerable<TResult> {}
}