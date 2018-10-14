using Super.Model.Selection;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Model.Sequences
{
	sealed class ArraySequence<_, T> : DecoratedSource<ISelect<_, T[]>>, ISequence<_, T>, IActivateMarker<ISelect<_, Store<T>>>
	{
		readonly ISelect<_, Store<T>> _select;
		readonly IBuilder<T>          _builder;

		public ArraySequence(ISelect<_, Store<T>> select) : this(@select, ArrayBuilder<T>.Default) {}

		public ArraySequence(ISelect<_, Store<T>> select, IBuilder<T> builder) : base(builder.Select(select.Select))
		{
			_select  = @select;
			_builder = builder;
		}

		public ISequence<_, T> Get(IAlterSelection parameter) => new ArraySequence<_, T>(_select, _builder.Get(parameter));

		public ISequence<_, T> Get(ISegment<T> parameter) => new ArraySequence<_, T>(_select, _builder.Get(parameter));
	}
}