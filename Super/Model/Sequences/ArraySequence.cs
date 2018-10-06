using Super.Model.Collections;
using Super.Model.Selection;

namespace Super.Model.Sequences
{
	sealed class ArraySequence<_, T> : ISequence<_, T>
	{
		readonly ISelect<_, Store<T>> _select;
		readonly IBuilder<T>          _builder;

		public ArraySequence(ISelect<_, Store<T>> select) : this(@select, ArrayBuilder<T>.Default) {}

		public ArraySequence(ISelect<_, Store<T>> select, IBuilder<T> builder)
		{
			_select  = @select;
			_builder = builder;
		}

		public ISelect<_, T[]> Get() => _select.Select(_builder.Get());

		public ISequence<_, T> Get(IAlterSelection parameter)
			=> new ArraySequence<_, T>(_select, _builder.Get(parameter));

		public ISequence<_, T> Get(ISegment<T> parameter) => new ArraySequence<_, T>(_select, _builder.Get(parameter));
	}
}