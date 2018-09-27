using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Structure;
using Super.Model.Sources;
using System;

namespace Super.Model.Sequences
{
	public interface ISegment<T> : IStructure<ArrayView<T>> {}

	sealed class Segment<T> : ISegment<T>
	{
		public static Segment<T> Default { get; } = new Segment<T>();

		Segment() {}

		public ArrayView<T> Get(in ArrayView<T> parameter) => parameter;
	}

	sealed class ArrayQuery<_, T> : FixedDeferredSingleton<Definition<T>, ISelect<_, T[]>>, IQuery<_, T>
	{
		readonly ISelect<_, T[]> _select;
		readonly IBuilder<_, T>  _builder;

		public ArrayQuery(ISelect<_, T[]> select) : this(@select, new Builder(select), Definition<T>.Default) {}

		public ArrayQuery(ISelect<_, T[]> select, IBuilder<_, T> builder, Definition<T> definition)
			: base(builder, definition)
		{
			_select    = @select;
			_builder   = builder;
			Definition = definition;
		}

		public Definition<T> Definition { get; }

		public IQuery<_, T> Get(Definition<T> parameter) => new ArrayQuery<_, T>(_select, _builder, parameter);

		sealed class Builder : Builder<_, T>
		{
			public Builder(ISelect<_, T[]> select)
				: base(definition => new SegmentedArray<T>(definition.Segment, definition.Selection), select) {}
		}
	}

	public interface IBuilder<in _, T> : ISelect<Definition<T>, ISelect<_, T[]>> {}

	class Builder<_, T> : IBuilder<_, T>
	{
		readonly ISelect<Definition<T>, IAlteration<T[]>> _alteration;
		readonly ISelect<_, T[]> _select;

		public Builder(Func<Definition<T>, IAlteration<T[]>> alteration, ISelect<_, T[]> select)
			: this(new Select<Definition<T>, IAlteration<T[]>>(alteration), @select) {}

		public Builder(ISelect<Definition<T>, IAlteration<T[]>> alteration, ISelect<_, T[]> select)
		{
			_alteration = alteration;
			_select = @select;
		}

		public ISelect<_, T[]> Get(Definition<T> parameter)
			=> parameter != Definition<T>.Default
				   ? _select.Select(parameter.Segment != Segment<T>.Default
					                    ? _alteration.Get(parameter)
					                    : new Clone<T>(Allocated<T>.Default, parameter.Selection))
				   : _select;
	}

	public sealed class Definition<T>
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