using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Structure;
using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	/*public readonly struct ArrayResultView<_, T>
	{
		public ArrayResultView(ISelect<_, IEnumerable<T>> source, IStores<T> stores)
			: this(source, stores, Result<T>.Default, Selection.Default) {}

		// ReSharper disable once TooManyDependencies
		public ArrayResultView(ISelect<_, IEnumerable<T>> source, IStores<T> stores,
		                       IStructure<ArrayView<T>, T[]> result, Selection selection)
		{
			Source    = source;
			Stores    = stores;
			Result    = result;
			Selection = selection;
		}

		public ISelect<_, IEnumerable<T>> Source { get; }

		public IStructure<ArrayView<T>, T[]> Result { get; }

		public Selection Selection { get; }

		public IStores<T> Stores { get; }
	}*/

	sealed class Result<T> : IStructure<ArrayView<T>, T[]>
	{
		public static Result<T> Default { get; } = new Result<T>();

		Result() : this(Allocated<T>.Default) {}

		readonly IStores<T> _stores;

		public Result(IStores<T> stores) => _stores = stores;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(in ArrayView<T> parameter)
			=> parameter.Start == 0 && parameter.Length == parameter.Array.Length
				   ? parameter.Array
				   : Fill(in parameter);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		T[] Fill(in ArrayView<T> parameter)
		{
			var result = _stores.Get(parameter.Length);
			Array.Copy(parameter.Array, (int)parameter.Start, result, 0, (int)parameter.Length);
			return result;
		}
	}

	sealed class ArrayStore<T> : Select<T[], Store<T>>
	{
		public static ArrayStore<T> Default { get; } = new ArrayStore<T>();

		ArrayStore() : base(x => new Store<T>(x, (uint)x.Length)) {}
	}

	sealed class SelectView<T> : ISelectView<T>
	{
		public static SelectView<T> Default { get; } = new SelectView<T>();

		SelectView() {}

		public ArrayView<T> Get(in Store<T> parameter) => new ArrayView<T>(parameter.Instance, 0, parameter.Length);
	}

	sealed class Copy<T> : ISelectView<T>
	{
		public static Copy<T> Default { get; } = new Copy<T>();

		Copy() : this(Allotted<T>.Default) {}

		readonly IStores<T> _stores;

		public Copy(IStores<T> stores) => _stores = stores;

		public ArrayView<T> Get(in Store<T> parameter)
		{
			var store  = _stores.Get(parameter.Length);
			var result = new ArrayView<T>(store, 0, parameter.Length);
			Array.Copy(parameter.Instance, 0, store, 0, parameter.Length);
			return result;
		}
	}

	public interface ISelectView<T> : IStructure<Store<T>, ArrayView<T>> {}

	public sealed class Body<T>
	{
		public static Body<T> Default { get; } = new Body<T>();

		Body() : this(SelectView<T>.Default, Content<T>.Default) {}

		public Body(ISelectView<T> enter, Content<T> content, Complete<T> exit = null)
		{
			Enter   = enter;
			Content = content;
			Exit    = exit;
		}

		public ISelectView<T> Enter { get; }

		public Content<T> Content { get; }

		public Complete<T> Exit { get; }
	}

	public sealed class Content<T>
	{
		public static Content<T> Default { get; } = new Content<T>();

		Content() : this(Start.Structure<ArrayView<T>>()) {}

		public Content(IStructure<ArrayView<T>, ArrayView<T>> select, Selection? selection = null)
		{
			Select    = @select;
			Selection = selection;
		}

		public IStructure<ArrayView<T>, ArrayView<T>> Select { get; }

		public Selection? Selection { get; }
	}

	public interface IContentComposer<T> : ISelect<Content<T>, IStructure<ArrayView<T>, ArrayView<T>>> {}

	sealed class ContentComposer<T> : IContentComposer<T>
	{
		public static ContentComposer<T> Default { get; } = new ContentComposer<T>();

		ContentComposer() {}

		public IStructure<ArrayView<T>, ArrayView<T>> Get(Content<T> parameter)
			=> parameter.Selection == null
				   ? parameter.Select
				   : parameter.Select.Select(new SelectionSegment<T>(parameter.Selection.Value));
	}

	public interface IBodyComposer<T> : ISelect<Body<T>, IStructure<Store<T>, ArrayView<T>>> {}

	sealed class BodyComposer<T> : IBodyComposer<T>
	{
		public static BodyComposer<T> Default { get; } = new BodyComposer<T>();

		BodyComposer() : this(ContentComposer<T>.Default) {}

		readonly IContentComposer<T> _content;

		public BodyComposer(IContentComposer<T> content) => _content = content;

		public IStructure<Store<T>, ArrayView<T>> Get(Body<T> parameter)
		{
			var content = _content.Get(parameter.Content);
			var select  = parameter.Enter.Select(content);
			var result  = parameter.Exit != null ? select.Select(new Completed<T>(parameter.Exit)) : select;
			return result;
		}
	}

	public sealed class Definition<T>
	{
		public static Definition<T> Default { get; } = new Definition<T>();

		Definition() : this(Body<T>.Default, Result<T>.Default) {}

		public Definition(Body<T> body, IStructure<ArrayView<T>, T[]> result)
		{
			Body   = body;
			Result = result;
		}

		public Body<T> Body { get; }

		public IStructure<ArrayView<T>, T[]> Result { get; }
	}

	public readonly struct Store<T>
	{
		public Store(T[] instance, uint length)
		{
			Instance = instance;
			Length   = length;
		}

		public T[] Instance { get; }

		public uint Length { get; }
	}

	sealed class ArrayComposition<_, T> : Composition<_, T>
	{
		public ArrayComposition(ISelect<_, T[]> enter) : base(enter.Select(ArrayStore<T>.Default)) {}
	}

	public class Composition<_, T>
	{
		public Composition(ISelect<_, Store<T>> enter, Complete<T> complete = null)
			: this(enter, Definition<T>.Default, complete) {}

		public Composition(ISelect<_, Store<T>> enter, Definition<T> definition, Complete<T> complete = null)
		{
			Enter      = enter;
			Definition = definition;
			Complete   = complete;
		}

		public ISelect<_, Store<T>> Enter { get; }

		public Definition<T> Definition { get; }

		public Complete<T> Complete { get; }
	}

	sealed class Completed<T> : IStructure<ArrayView<T>>
	{
		readonly Complete<T> _complete;

		public Completed(Complete<T> complete) => _complete = complete;

		public ArrayView<T> Get(in ArrayView<T> parameter)
		{
			_complete(parameter.Array);
			return parameter;
		}
	}

	sealed class Returned<T> : IStructure<Store<T>, T[]>
	{
		readonly Selection<Store<T>, T[]> _reference;
		readonly Complete<T>              _complete;

		public Returned(Selection<Store<T>, T[]> reference, Complete<T> complete)
		{
			_reference = reference;
			_complete  = complete;
		}

		public T[] Get(in Store<T> parameter)
		{
			var result = _reference(parameter);
			_complete(parameter.Instance);
			return result;
		}
	}

	public interface IDefinitionComposer<T> : ISelect<Definition<T>, IStructure<Store<T>, T[]>> {}

	sealed class DefinitionComposer<T> : IDefinitionComposer<T>
	{
		public static DefinitionComposer<T> Default { get; } = new DefinitionComposer<T>();

		DefinitionComposer() : this(BodyComposer<T>.Default) {}

		readonly IBodyComposer<T> _body;

		public DefinitionComposer(IBodyComposer<T> body) => _body = body;

		public IStructure<Store<T>, T[]> Get(Definition<T> parameter) => _body.Get(parameter.Body)
		                                                                      .Select(parameter.Result);
	}

	sealed class Composer<_, T> : ISelect<Composition<_, T>, ISelect<_, T[]>>
	{
		public static Composer<_, T> Default { get; } = new Composer<_, T>();

		Composer() : this(DefinitionComposer<T>.Default) {}

		readonly IDefinitionComposer<T> _definition;

		public Composer(IDefinitionComposer<T> definition) => _definition = definition;

		public ISelect<_, T[]> Get(Composition<_, T> parameter)
		{
			var definition = _definition.Get(parameter.Definition);
			var contents = parameter.Complete != null
				               ? new Returned<T>(definition.Get, parameter.Complete)
				               : definition;
			var result = parameter.Enter.Select(contents);
			return result;
		}
	}

	public delegate void Complete<in T>(T[] resource);

	public interface IDefinitionAlteration<T> : IAlteration<Definition<T>> {}

	sealed class Skip<T> : IContentAlteration<T>
	{
		readonly uint _skip;

		public Skip(uint skip) => _skip = skip;

		public Content<T> Get(Content<T> parameter)
		{
			var current   = parameter.Selection ?? Selection.Default;
			var selection = new Selection(current.Start + _skip, current.Length - _skip);
			var result    = new Content<T>(parameter.Select, selection);
			return result;
		}
	}

	sealed class Take<T> : IContentAlteration<T>
	{
		readonly uint _take;

		public Take(uint take) => _take = take;

		public Content<T> Get(Content<T> parameter)
		{
			var current = parameter.Selection ?? Selection.Default;
			var result  = new Content<T>(parameter.Select, new Selection(current.Start, _take));
			return result;
		}
	}

	sealed class WhereDefinition<T> : AllottedDefinitionAlteration<T>
	{
		public WhereDefinition(Func<T, bool> where) : base(new WhereSegment<T>(where)) {}
	}

	class DefinitionAlteration<T> : IDefinitionAlteration<T>
	{
		readonly IBodyAlteration<T> _alteration;

		public DefinitionAlteration(ISelectView<T> enter, ISegment<T> select, Complete<T> exit = null)
			: this(new BodyContentAlteration<T>(new SegmentAlteration<T>(@select), enter, exit)) {}

		public DefinitionAlteration(IBodyAlteration<T> alteration) => _alteration = alteration;

		public Definition<T> Get(Definition<T> parameter)
			=> new Definition<T>(_alteration.Get(parameter.Body), parameter.Result);
	}

	class AllottedDefinitionAlteration<T> : DefinitionAlteration<T>
	{
		readonly static Complete<T> Complete = Allotted<T>.Default.Execute;

		public AllottedDefinitionAlteration(ISegment<T> segment): base(Copy<T>.Default, segment, Complete) {}
	}

	public interface IBodyAlteration<T> : IAlteration<Body<T>> {}

	public interface IContentAlteration<T> : IAlteration<Content<T>> {}

	class SegmentAlteration<T> : IContentAlteration<T>
	{
		readonly ISegment<T> _select;
		readonly Selection?  _selection;

		public SegmentAlteration(ISegment<T> select, Selection? selection = null)
		{
			_select    = @select;
			_selection = selection;
		}

		public Content<T> Get(Content<T> parameter) => new Content<T>(parameter.Select.Select(_select), _selection);
	}

	sealed class BodyContentAlteration<T> : IBodyAlteration<T>
	{
		readonly IContentAlteration<T> _content;
		readonly ISelectView<T>        _enter;
		readonly Complete<T>           _exit;

		public BodyContentAlteration(IContentAlteration<T> content, ISelectView<T> enter = null,
		                             Complete<T> exit = null)
		{
			_content = content;
			_enter   = enter;
			_exit    = exit;
		}

		public Body<T> Get(Body<T> parameter)
			=> new Body<T>(_enter ?? parameter.Enter, _content.Get(parameter.Content), _exit ?? parameter.Exit);
	}

	sealed class SelectionSegment<T> : ISegment<T>
	{
		readonly Selection _selection;

		public SelectionSegment(Selection selection) => _selection = selection;

		public ArrayView<T> Get(in ArrayView<T> parameter)
			=> parameter.Resize(_selection.Start, _selection.Length ?? parameter.Length - _selection.Start);
	}

	sealed class WhereSegment<T> : ISegment<T>
	{
		readonly Func<T, bool> _where;

		public WhereSegment(Func<T, bool> where) => _where = where;

		public ArrayView<T> Get(in ArrayView<T> parameter)
		{
			var used  = parameter.Length;
			var array = parameter.Array;
			var count = 0u;
			for (var i = 0u; i < used; i++)
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