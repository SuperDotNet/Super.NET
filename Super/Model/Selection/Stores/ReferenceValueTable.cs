using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Selection.Stores
{
	public class AssociatedResource<T> : AssociatedResource<object, T>
	{
		
	}

	public class AssociatedResource<TParameter, TResult> : DecoratedTable<TParameter, TResult>
	{
		public AssociatedResource()
			: base(Tables<TParameter, TResult>.Default.Get(Activator<TResult>
			                                               .Default.Allow(I<TParameter>.Default)
			                                               .ToDelegate())) {}
	}

	public class ReferenceValueTable<TParameter, TResult> : DecoratedTable<TParameter, TResult>
		where TParameter : class
		where TResult : class
	{
		public ReferenceValueTable() : this(_ => default) {}

		public ReferenceValueTable(Func<TParameter, TResult> parameter)
			: base(ReferenceValueTables<TParameter, TResult>.Defaults
			                                                .Get(parameter)
			                                                .Get(new ConditionalWeakTable<TParameter, TResult>())) {}
	}
}