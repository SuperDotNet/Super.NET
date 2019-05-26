using System;
using System.Threading.Tasks;
using Super.Model.Selection;

namespace Super.Operations
{
	public sealed class Handle<TIn, TOut> : ISelect<Task<TIn>, TOut>
	{
		readonly Func<TIn, TOut> _select;

		public Handle(Func<TIn, TOut> select) => _select = select;

		public TOut Get(Task<TIn> parameter) => _select(parameter.Result);
	}
}