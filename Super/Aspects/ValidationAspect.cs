using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sequences;
using Super.Runtime;
using Super.Runtime.Environment;
using Super.Runtime.Invocation;
using System;

namespace Super.Aspects
{
	public sealed class Aspects<TIn, TOut> : SystemRegistry<IAspect<TIn, TOut>>
	{
		public static Aspects<TIn, TOut> Default { get; } = new Aspects<TIn, TOut>();

		Aspects() : base(DefaultAspects<TIn, TOut>.Default) {}
	}

	public sealed class DefaultAspects<TIn, TOut> : SystemStore<Array<IAspect<TIn, TOut>>>
	{
		public static DefaultAspects<TIn,TOut> Default { get; } = new DefaultAspects<TIn,TOut>();

		DefaultAspects() : this(AssignedAspect<TIn, TOut>.Default) {}

		public DefaultAspects(params IAspect<TIn, TOut>[] elements) : this(new Array<IAspect<TIn, TOut>>(elements)) {}

		public DefaultAspects(Array<IAspect<TIn, TOut>> elements)
			: base(new ArrayInstance<IAspect<TIn, TOut>>(elements)) {}
	}

	public static class Extensions
	{
		public static ISelect<TIn, TOut> Featured<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> Apply<TIn, TOut>.Default.Get(@this);
	}

	public sealed class Apply<TIn, TOut> : IAspect<TIn, TOut>
	{
		public static Apply<TIn, TOut> Default { get; } = new Apply<TIn, TOut>();

		Apply() : this(Aspects<TIn, TOut>.Default) {}

		readonly IArray<IAspect<TIn, TOut>> _items;

		public Apply(IArray<IAspect<TIn, TOut>> items) => _items = items;

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter)
		{
			var items  = _items.Get();
			var count  = items.Length;
			var result = parameter;
			for (var i = 0u; i < count; i++)
			{
				result = items[i].Get(result);
			}
			return result;
		}
	}

	public interface IAspect<TIn, TOut> : IAlteration<ISelect<TIn, TOut>> {}

	public sealed class AssignedAspect<TIn, TOut> : ValidationAspect<TIn, TOut>
	{
		public static AssignedAspect<TIn, TOut> Default { get; } = new AssignedAspect<TIn, TOut>();

		AssignedAspect() : base(DefaultGuard<TIn>.Default.Execute) {}
	}

	public class ValidationAspect<TIn, TOut> : Invocation0<ISelect<TIn, TOut>, Action<TIn>, ISelect<TIn, TOut>>,
	                                           IAspect<TIn, TOut>
	{
		public ValidationAspect(Action<TIn> parameter)
			: base((select, action) => new Configured<TIn, TOut>(select, action), parameter) {}
	}
}