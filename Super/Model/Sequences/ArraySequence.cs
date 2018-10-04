using Super.Model.Collections;
using Super.Model.Selection;

namespace Super.Model.Sequences
{
	sealed class ArraySequence<_, T> : ISequence<_, T>
	{
		readonly ISelect<_, T[]> _select;
		readonly IBuilder<T[], T> _builder;

		public ArraySequence(ISelect<_, T[]> select) : this(select, ArrayBuilder<T>.Default) {}

		public ArraySequence(ISelect<_, T[]> select, IBuilder<T[], T> builder)
		{
			_select = @select;
			_builder = builder;
		}

		public ISelect<_, T[]> Get() => _select.Select(_builder.Get());

		public ISequence<_, T> Get(IState<Collections.Selection> parameter)
			=> new ArraySequence<_, T>(_select, _builder.Get(parameter));

		public ISequence<_, T> Get(ISegment<T> parameter) => new ArraySequence<_, T>(_select, _builder.Get(parameter));
	}
}