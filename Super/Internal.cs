using Super.Model.Selection;
using Super.Model.Selection.Stores;

namespace Super
{
	static class ExtensionMethodsInternal
	{
		public static ISelect<TParameter, TResult> ToReferenceStore<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this)
			where TResult : class
			where TParameter : class
			=> DefaultReferenceValueTables<TParameter, TResult>.Default.Get(@this.ToDelegate());
	}
}