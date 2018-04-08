using Super.Model.Selection;
using Super.Model.Selection.Stores;

namespace Super.ExtensionMethods
{
	static class Internal
	{
		public static ISelect<TParameter, TResult> ToReferenceStore<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this)
			where TResult : class
			where TParameter : class
			=> DefaultReferenceValueTables<TParameter, TResult>.Default.Get(@this.ToDelegate());
	}
}