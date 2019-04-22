using Super.Model.Selection;
using System;
using System.Threading.Tasks;

namespace Super.Services
{
	public static class ExtensionMethods
	{
		public static ISelect<TIn, TOut> Request<TIn, T, TOut>(this ISelect<TIn, T> @this,
		                                                       Func<T, Task<TOut>> parameter)
			=> @this.Select(new Request<T, TOut>(parameter))
			        .Select(Request<TOut>.Default);
	}
}