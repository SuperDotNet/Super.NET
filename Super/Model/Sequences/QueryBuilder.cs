using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Structure;
using Super.Model.Sources;

namespace Super.Model.Sequences
{
	public interface ISelection<T> : IStructure<ArrayView<T>, Session<T>> {}

	public interface ISegment<T> : IStructure<ArrayView<T>> {}

	sealed class DefaultSegment<T> : ISegment<T>
	{
		public static DefaultSegment<T> Default { get; } = new DefaultSegment<T>();

		DefaultSegment() {}

		public ArrayView<T> Get(in ArrayView<T> parameter) => parameter;
	}

	sealed class Segment<T> : ISegment<T>
	{
		readonly Selection<ArrayView<T>, ArrayView<T>> _select;

		public Segment(Selection<ArrayView<T>, ArrayView<T>> @select) => _select = @select;

		public ArrayView<T> Get(in ArrayView<T> parameter) => _select(parameter);
	}

	sealed class ArrayQuery<_, T> : FixedDeferredSingleton<ISelection<T>, ISelect<_, T[]>>, IQuery<_, T>
	{
		readonly ISelect<_, T[]> _select;
		readonly IBuilder<_, T>  _builder;

		public ArrayQuery(ISelect<_, T[]> select) : this(@select, new Builder(select)) {}

		public ArrayQuery(ISelect<_, T[]> select, IBuilder<_, T> builder) : base(builder, null)
		{
			_select  = @select;
			_builder = builder;
		}

		public IQuery<_, T> Get(IAlterSelection<T> parameter)
			=> new ArrayQuery<_, T>(_select, _builder);

		sealed class Builder : Builder<_, T>
		{
			public Builder(ISelect<_, T[]> select) : base(select) {}
		}
	}

	public interface IBuilder<in _, T> : ISelect<ISelection<T>, ISelect<_, T[]>> {}

	class Builder<_, T> : IBuilder<_, T>
	{
		readonly ISelect<_, T[]> _select;

		public Builder(ISelect<_, T[]> select) => _select = @select;

		public ISelect<_, T[]> Get(ISelection<T> parameter) => _select.Select(DefaultArraySelection<T>.Default);
	}
}