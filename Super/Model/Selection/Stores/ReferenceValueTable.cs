using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Selection.Stores
{
	public class AssociatedResource<TParameter, TResult> : DecoratedTable<TParameter, TResult>
	{
		readonly static Func<TParameter, TResult> Resource = Activator<TResult>.Default
		                                                  .Allow(I<TParameter>.Default)
		                                                  .ToDelegate();

		public AssociatedResource() : this(Resource) {}

		public AssociatedResource(Func<TParameter, TResult> resource)
			: base(Tables<TParameter, TResult>.Default.Get(resource)) {}
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