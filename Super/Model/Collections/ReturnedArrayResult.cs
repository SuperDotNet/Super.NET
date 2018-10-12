using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Runtime;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface IStoreReferences<T> : ISelect<Store<T>, T[]> {}

	/*sealed class StoreReferences<T> : IStoreReferences<T>
	{
		public static StoreReferences<T> Default { get; } = new StoreReferences<T>();

		StoreReferences() : this(Allocated<T>.Default) {}

		readonly IStores<T> _stores;

		public StoreReferences(IStores<T> stores) => _stores = stores;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(in Store<T> parameter)
			=> parameter.Length == parameter.Instance.Length
				   ? parameter.Instance
				   : parameter.CopyInto(_stores.Get(parameter.Length));
	}*/

	public interface IReferences<T> : ISelect<ArrayView<T>, T[]> {}

	sealed class References<T> : IReferences<T>
	{
		public static References<T> Default { get; } = new References<T>();

		References() : this(Allocated<T>.Default) {}

		readonly IStores<T> _stores;

		public References(IStores<T> stores) => _stores = stores;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(ArrayView<T> parameter)
			=> parameter.Start == 0 && parameter.Length == parameter.Array.Length
				   ? parameter.Array
				   : parameter.Into(_stores.Get(parameter.Length).Instance);
	}

	sealed class ArrayStores<T> : ISelect<T[], ArrayView<T>>
	{
		public static ArrayStores<T> Default { get; } = new ArrayStores<T>();

		ArrayStores() : this(Selection.Default) {}

		readonly Selection _selection;

		public ArrayStores(Selection selection) => _selection = selection;

		public ArrayView<T> Get(T[] parameter)
			=> new ArrayView<T>(parameter, _selection.Start,
			                    _selection.Length.IsAssigned ? _selection.Length.Instance : (uint)parameter.Length - _selection.Start);
	}

	sealed class RuntimeStores<T> : ISelect<IEnumerable<T>, ArrayView<T>>, IActivateMarker<Selection>
	{
		readonly Func<T[], ArrayView<T>>            _array;
		readonly Func<IEnumerator<T>, ArrayView<T>> _enumerate;

		public RuntimeStores(Selection selection)
			: this(new ArrayStores<T>(selection).Get, new Enumerate<T>(selection).Get) {}

		public RuntimeStores(Func<T[], ArrayView<T>> array, Func<IEnumerator<T>, ArrayView<T>> enumerate)
		{
			_array     = array;
			_enumerate = enumerate;
		}

		public ArrayView<T> Get(IEnumerable<T> parameter)
		{
			switch (parameter)
			{
				case T[] array:
					return _array(array);
				default:
					return _enumerate(parameter.GetEnumerator());
			}
		}
	}

	sealed class EnumerableStores<T> : ISelect<IEnumerable<T>, ArrayView<T>>, IActivateMarker<Selection>
	{
		public static EnumerableStores<T> Default { get; } = new EnumerableStores<T>();

		EnumerableStores() : this(Enumerate<T>.Default.Get) {}

		readonly Func<IEnumerator<T>, ArrayView<T>> _enumerate;

		public EnumerableStores(Selection selection) : this(new Enumerate<T>(selection).Get) {}

		public EnumerableStores(Func<IEnumerator<T>, ArrayView<T>> enumerate) => _enumerate = enumerate;

		public ArrayView<T> Get(IEnumerable<T> parameter) => _enumerate(parameter.GetEnumerator());
	}

	sealed class SelectView<T> : ISelectView<T>
	{
		public static SelectView<T> Default { get; } = new SelectView<T>();

		SelectView() {}

		public ArrayView<T> Get(ArrayView<T> parameter) => parameter;
	}

	/*sealed class Copy<T> : ISelectView<T>
	{
		public static Copy<T> Default { get; } = new Copy<T>();

		Copy() : this(Allotted<T>.Default) {}

		readonly IStores<T> _stores;

		public Copy(IStores<T> stores) => _stores = stores;

		public ArrayView<T> Get(in ArrayView<T> parameter)
			=> new ArrayView<T>(parameter.Into(_stores.Get(parameter.Length).Instance), 0, parameter.Length);
	}*/

	public interface ISelectView<T> : ISelect<ArrayView<T>, ArrayView<T>> {}

	public interface IBodyAlteration<T> : IAlteration<Body<T>> {}

	sealed class BodyContentAlteration<T> : IBodyAlteration<T>
	{
		readonly IContentAlteration<T> _alteration;

		public BodyContentAlteration(IContentAlteration<T> alteration) => _alteration = alteration;

		public Body<T> Get(Body<T> parameter) => new Body<T>(_alteration.Get(parameter.Content), parameter.Selection);
	}

	public sealed class Body<T>
	{
		public static Body<T> Default { get; } = new Body<T>();

		Body() : this(Content<T>.Default) {}

		public Body(Content<T> content, Selection? selection = null)
		{
			Content   = content;
			Selection = selection;
		}

		public Content<T> Content { get; }

		public Selection? Selection { get; }
	}

	public sealed class Content<T>
	{
		public static Content<T> Default { get; } = new Content<T>();

		Content() : this(SelectView<T>.Default, In<ArrayView<T>>.Start()) {}

		public Content(ISelectView<T> enter, ISelect<ArrayView<T>, ArrayView<T>> @select, Complete<T> exit = null)
		{
			Enter  = enter;
			Select = @select;
			Exit   = exit;
		}

		public ISelectView<T> Enter { get; }

		public ISelect<ArrayView<T>, ArrayView<T>> Select { get; }

		public Complete<T> Exit { get; }
	}

	public interface IContentComposer<T> : ISelect<Content<T>, ISelect<ArrayView<T>, ArrayView<T>>> {}

	sealed class ContentComposer<T> : IContentComposer<T>
	{
		public static ContentComposer<T> Default { get; } = new ContentComposer<T>();

		ContentComposer() {}

		public ISelect<ArrayView<T>, ArrayView<T>> Get(Content<T> parameter)
		{
			var select = parameter.Enter.Select(parameter.Select);
			var result = parameter.Exit != null ? select.Select(new Completed<T>(parameter.Exit)) : select;
			return result;
		}
	}

	public readonly struct Store<T>
	{
		public static Store<T> Empty { get; } = new Store<T>(Empty<T>.Array);

		public Store(T[] instance) : this(instance, (uint)instance.Length) {}

		public Store(T[] instance, uint length)
		{
			Instance = instance;
			Length   = length;
		}

		public T[] Instance { get; }

		public uint Length { get; }
	}

	public interface IStart<in _, T> : ISelect<Selection?, ISelect<_, ArrayView<T>>> {}

	sealed class ArrayDefinition<_, T> : Definition<_, T>
	{
		public ArrayDefinition(ISelect<_, T[]> enter) : base(new Entrance(enter)) {}

		sealed class Entrance : IStart<_, T>
		{
			readonly ISelect<_, T[]> _select;

			public Entrance(ISelect<_, T[]> @select) => _select = @select;

			public ISelect<_, ArrayView<T>> Get(Selection? parameter)
				=> _select.Select(parameter.HasValue ? new ArrayStores<T>(parameter.Value) : ArrayStores<T>.Default);
		}
	}

	sealed class EnumerableDefinition<_, T> : Definition<_, T>
	{
		public EnumerableDefinition(ISelect<_, IEnumerable<T>> enter)
			: base(new Entrance(enter), Return.Default.Execute) {}

		sealed class Entrance : IStart<_, T>
		{
			readonly ISelect<_, IEnumerable<T>> _select;

			public Entrance(ISelect<_, IEnumerable<T>> @select) => _select = @select;

			public ISelect<_, ArrayView<T>> Get(Selection? parameter)
				=> _select.Select(parameter.HasValue
					                  ? new EnumerableStores<T>(parameter.Value)
					                  : EnumerableStores<T>.Default);
		}

		sealed class Return : ValidatedCommand<T[]>
		{
			public static Return Default { get; } = new Return();

			Return() : base(In<T[]>.Is(x => x.Length > 4), Allotted<T>.Default) {}
		}
	}

	public class Definition<_, T>
	{
		public Definition(IStart<_, T> start, Complete<T> complete = null) : this(start, Body<T>.Default, complete) {}

		public Definition(IStart<_, T> start, Body<T> body, Complete<T> complete = null)
		{
			Start    = start;
			Body     = body;
			Complete = complete;
		}

		public IStart<_, T> Start { get; }

		public Body<T> Body { get; }

		public Complete<T> Complete { get; }
	}

	sealed class Completed<T> : IAlteration<ArrayView<T>>
	{
		readonly Complete<T> _complete;

		public Completed(Complete<T> complete) => _complete = complete;

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			_complete(parameter.Array);
			return parameter;
		}
	}

	sealed class Returned<T> : ISelect<ArrayView<T>, ArrayView<T>>
	{
		readonly Func<ArrayView<T>, ArrayView<T>> _reference;
		readonly Complete<T>                           _complete;

		public Returned(Func<ArrayView<T>, ArrayView<T>> reference, Complete<T> complete)
		{
			_reference = reference;
			_complete  = complete;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var result = _reference(parameter);
			_complete(parameter.Array);
			return result;
		}
	}

	sealed class Composer<_, T> : ISelect<Definition<_, T>, ISelect<_, ArrayView<T>>>
	{
		public static Composer<_, T> Default { get; } = new Composer<_, T>();

		Composer() : this(ContentComposer<T>.Default) {}

		readonly IContentComposer<T> _content;

		public Composer(IContentComposer<T> content) => _content = content;

		public ISelect<_, ArrayView<T>> Get(Definition<_, T> parameter)
		{
			var current   = parameter.Body.Content;
			var modified  = current != Content<T>.Default;
			var selection = parameter.Body.Selection;
			var selected  = selection.HasValue;

			var accounted = modified && selected
				                ? new Content<T>(current.Enter,
				                                 current.Select.Select(new SelectionSegment<T>(selection.Value)),
				                                 current.Exit)
				                : current;

			var content = _content.Get(accounted);
			var contents = parameter.Complete != null
				               ? new Returned<T>(content.Get, parameter.Complete)
				               : content;
			var start  = parameter.Start.Get(modified ? null : selection);
			var result = start.Select(contents);
			return result;
		}
	}

	public delegate void Complete<in T>(T[] resource);

	sealed class Skip<T> : IBodyAlteration<T>
	{
		readonly uint _skip;

		public Skip(uint skip) => _skip = skip;

		public Body<T> Get(Body<T> parameter)
		{
			var current   = parameter.Selection ?? Selection.Default;
			var selection = new Selection(current.Start + _skip, current.Length - _skip);
			var result    = new Body<T>(parameter.Content, selection);
			return result;
		}
	}

	sealed class Take<T> : IBodyAlteration<T>
	{
		readonly uint _take;

		public Take(uint take) => _take = take;

		public Body<T> Get(Body<T> parameter)
		{
			var current = parameter.Selection ?? Selection.Default;
			var result  = new Body<T>(parameter.Content, new Selection(current.Start, _take));
			return result;
		}
	}

	sealed class WhereSelection<T> : ContentAlteration<T>
	{
		public WhereSelection(Func<T, bool> where) : base(new WhereSegment<T>(where)) {}
	}

	/*class AllottedContentAlteration<T> : ContentAlteration<T>
	{
		readonly static Complete<T> Complete = Allotted<T>.Default.Execute;

		public AllottedContentAlteration(ISegment<T> segment) : base(segment, Copy<T>.Default, Complete) {}
	}*/

	public interface IContentAlteration<T> : IAlteration<Content<T>> {}

	class ContentAlteration<T> : IContentAlteration<T>
	{
		readonly ISelect<ArrayView<T>, ArrayView<T>> _content;
		readonly ISelectView<T>                         _enter;
		readonly Complete<T>                            _exit;

		public ContentAlteration(ISelect<ArrayView<T>, ArrayView<T>> content,
		                         ISelectView<T> enter = null,
		                         Complete<T> exit = null)
		{
			_content = content;
			_enter   = enter;
			_exit    = exit;
		}

		public Content<T> Get(Content<T> parameter)
			=> new Content<T>(_enter ?? parameter.Enter, parameter.Select.Select(_content), _exit ?? parameter.Exit);
	}

	sealed class SelectionSegment<T> : ISegment<T>
	{
		readonly Selection _selection;

		public SelectionSegment(Selection selection) => _selection = selection;

		public ArrayView<T> Get(ArrayView<T> parameter)
			=> parameter.Resize(_selection.Start, _selection.Length.IsAssigned ? _selection.Length.Instance : parameter.Length - _selection.Start);
	}

	sealed class WhereSegment<T> : ISegment<T>
	{
		readonly Func<T, bool> _where;

		public WhereSegment(Func<T, bool> where) => _where = @where;

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var used  = parameter.Length;
			var array = parameter.Array;
			var count = 0u;
			for (var i = 0; i < used; i++)
			{
				var item = array[i];
				if (_where(item))
				{
					array[count++] = item;
				}
			}

			return parameter.Resize(count);
		}
	}
}