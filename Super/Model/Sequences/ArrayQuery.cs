using Super.Model.Collections;
using Super.Model.Selection;

namespace Super.Model.Sequences
{
	sealed class ArrayQuery<_, T> : IQuery<_, T>
	{
		readonly ISelect<_, T[]> _select;
		readonly IBuilder<T> _builder;

		public ArrayQuery(ISelect<_, T[]> select) : this(select, Builder<T>.Default) {}

		public ArrayQuery(ISelect<_, T[]> select, IBuilder<T> builder)
		{
			_select = @select;
			_builder = builder;
		}

		public ISelect<_, T[]> Get() => _select.Select(_builder.Get());

		public IQuery<_, T> Get(IAlterState<Collections.Selection> parameter)
			=> new ArrayQuery<_, T>(_select, _builder.Get(parameter));

		public IQuery<_, T> Get(ISegment<T> parameter) => new ArrayQuery<_, T>(_select, _builder.Get(parameter));
	}
}