using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Structure;
using Super.Model.Sources;

namespace Super.Model.Sequences
{
	public interface ISegment<T> : IStructure<ArrayView<T>> {}

	sealed class Segment<T> : ISegment<T>
	{
		public static Segment<T> Default { get; } = new Segment<T>();

		Segment() {}

		public ArrayView<T> Get(in ArrayView<T> parameter) => parameter;
	}

	sealed class ArrayQuery<_, T> : Query<_, T>
	{
		public ArrayQuery(ISelect<_, T[]> select) : base(new Selector(select)) {}

		sealed class Selector : ISelect<Definition<T>, ISelect<_, T[]>>
		{
			readonly ISelect<_, T[]> _select;

			public Selector(ISelect<_, T[]> select) => _select = @select;

			public ISelect<_, T[]> Get(Definition<T> parameter)
				=> parameter != Definition<T>.Default
					   ? _select.Select(parameter.Segment != Segment<T>.Default
						                    ? (IAlteration<T[]>)new SegmentedArray<T>(parameter.Segment, parameter.Selection)
						                    : new Clone<T>(Allocated<T>.Default, parameter.Selection))
					   : _select;
		}
	}

	class Query<_, T> : FixedDeferredSingleton<Definition<T>, ISelect<_, T[]>>
	{
		public Query(ISelect<Definition<T>, ISelect<_, T[]>> select) : this(@select, Definition<T>.Default) {}

		public Query(ISelect<Definition<T>, ISelect<_, T[]>> select, Definition<T> definition) :
			base(select, definition)
		{
			Select     = @select;
			Definition = definition;
		}

		public ISelect<Definition<T>, ISelect<_, T[]>> Select { get; }

		public Definition<T> Definition { get; }
	}

	sealed class Definition<T>
	{
		public static Definition<T> Default { get; } = new Definition<T>();

		Definition() : this(Segment<T>.Default, Collections.Selection.Default) {}

		public Definition(ISegment<T> segment, Collections.Selection selection)
		{
			Segment   = segment;
			Selection = selection;
		}

		public ISegment<T> Segment { get; }

		public Collections.Selection Selection { get; }
	}
}