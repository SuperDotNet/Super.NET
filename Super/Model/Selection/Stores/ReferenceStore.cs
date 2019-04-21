using System;

namespace Super.Model.Selection.Stores
{
	public class ReferenceValueStore<TIn, TOut> : DecoratedSelect<TIn, TOut> where TIn : class where TOut : class
	{
		protected ReferenceValueStore(Func<TIn, TOut> source) : base(new ReferenceValueTable<TIn, TOut>(source)) {}
	}

	public class StructureValueStore<TIn, TOut> : DecoratedSelect<TIn, TOut>
		where TIn : class
		where TOut : struct
	{
		protected StructureValueStore(Func<TIn, TOut> source) : base(new StructureValueTable<TIn, TOut>(source)) {}
	}
}