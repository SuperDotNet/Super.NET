using Super.ExtensionMethods;
using Super.Model.Specifications;
using Super.Reflection.Types;
using System;

namespace Super.Model.Selection.Stores
{
	public sealed class ReferenceTables<TParameter, TResult>
		: Delegated<Func<TParameter, TResult>, ITable<TParameter, TResult>>
		where TParameter : class
	{
		public static ReferenceTables<TParameter, TResult> Default { get; } = new ReferenceTables<TParameter, TResult>();

		ReferenceTables() : this(IsValueType.Default.IsSatisfiedBy(typeof(TResult))
			                         ? typeof(StructureValueTable<,>)
			                         : typeof(ReferenceValueTable<,>)) {}

		public ReferenceTables(Type type)
			: base(new Generic<Func<TParameter, TResult>, ITable<TParameter, TResult>>(type)
			       .Get(typeof(TParameter), typeof(TResult))
			       .Invoke) {}
	}
}