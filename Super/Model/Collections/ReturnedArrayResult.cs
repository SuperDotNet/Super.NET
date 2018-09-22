using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Structure;
using System;
using System.Collections.Generic;
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
			=> parameter.Start == 0 && parameter.Length == parameter.Array.Length ? parameter.Array : Fill(parameter);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		T[] Fill(in ArrayView<T> parameter)
		{
			var result = _stores.Get(parameter.Length);
			Array.ConstrainedCopy(parameter.Array, (int)parameter.Start, result, 0, (int)parameter.Length);
			return result;
		}
	}

	sealed class SelectView<T> : Select<T[], ArrayView<T>>
	{
		public static SelectView<T> Default { get; } = new SelectView<T>();

		SelectView() : base(x => new ArrayView<T>(x)) {}
	}

	sealed class AllottedCopy<T> : ISelect<T[], ArrayView<T>>
	{
		public static AllottedCopy<T> Default { get; } = new AllottedCopy<T>();

		AllottedCopy() : this(Allotted<T>.Default) {}

		readonly IStores<T> _stores;

		public AllottedCopy(IStores<T> stores) => _stores = stores;

		public ArrayView<T> Get(T[] parameter)
		{
			var store = _stores.Get(parameter.Length);
			var result = new ArrayView<T>(store, 0, (uint)parameter.Length);
			Array.ConstrainedCopy(parameter, 0, store, 0, parameter.Length);
			return result;
		}
	}

	public sealed class Body<T>
	{
		public static Body<T> Default { get; } = new Body<T>();

		Body() : this(SelectView<T>.Default) {}

		public Body(ISelect<T[], ArrayView<T>> enter, Complete<T> complete = null)
			: this(enter, Content<T>.Default, complete) {}

		public Body(ISelect<T[], ArrayView<T>> enter, Content<T> content, Complete<T> exit = null)
		{
			Enter   = enter;
			Content = content;
			Exit    = exit;
		}

		public ISelect<T[], ArrayView<T>> Enter { get; }

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

	public interface IBodyComposer<T> : ISelect<Body<T>, ISelect<T[], ArrayView<T>>> {}

	sealed class BodyComposer<T> : IBodyComposer<T>
	{
		public static BodyComposer<T> Default { get; } = new BodyComposer<T>();

		BodyComposer() : this(ContentComposer<T>.Default) {}

		readonly IContentComposer<T> _content;

		public BodyComposer(IContentComposer<T> content) => _content = content;

		public ISelect<T[], ArrayView<T>> Get(Body<T> parameter)
		{
			var content = _content.Get(parameter.Content);
			var select  = parameter.Enter.Select(content);
			var result  = parameter.Exit != null ? select.Select(new Completed<T>(parameter.Exit)) : select;
			return result;
		}
	}

	/*sealed class Entrance<_, T>
	{
		public Entrance(ISelect<_, IEnumerable<T>> enter, ISelect<_, T[]> select)
		{
			Enter  = enter;
			Select = @select;
		}

		public ISelect<_, IEnumerable<T>> Enter { get; }

		public ISelect<_, T[]> Select { get; }
	}*/

	public class Entrance<T>
	{
		public Entrance(ISelect<IEnumerable<T>, T[]> select, Complete<T> complete = null)
		{
			Select   = @select;
			Complete = complete;
		}

		public ISelect<IEnumerable<T>, T[]> Select { get; }

		public Complete<T> Complete { get; }
	}

	public sealed class Definition<T>
	{
		public Definition(Entrance<T> enter) : this(enter, Body<T>.Default, Result<T>.Default) {}

		public Definition(Entrance<T> enter, Body<T> body, IStructure<ArrayView<T>, T[]> result)
		{
			Enter  = enter;
			Body   = body;
			Result = result;
		}

		public Entrance<T> Enter { get; }

		public Body<T> Body { get; }

		public IStructure<ArrayView<T>, T[]> Result { get; }
	}

	public sealed class Composition<_, T>
	{
		public Composition(ISelect<_, IEnumerable<T>> enter, Definition<T> definition)
		{
			Enter      = enter;
			Definition = definition;
		}

		public ISelect<_, IEnumerable<T>> Enter { get; }

		public Definition<T> Definition { get; }
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

	class Compositions<_, T> : ISelect<ISelect<_, IEnumerable<T>>, Composition<_, T>>
	{
		readonly Entrance<T> _entrance;

		public Compositions(ISelect<IEnumerable<T>, T[]> select, Complete<T> complete = null)
			: this(new Entrance<T>(select, complete)) {}

		public Compositions(Entrance<T> entrance) => _entrance = entrance;

		public Composition<_, T> Get(ISelect<_, IEnumerable<T>> parameter)
			=> new Composition<_, T>(parameter, new Definition<T>(_entrance));
	}

	sealed class ArrayCompositions<_, T> : Compositions<_, T>
	{
		public static ArrayCompositions<_,T> Default { get; } = new ArrayCompositions<_,T>();

		ArrayCompositions() : base(In<IEnumerable<T>>.Select(x => (T[])x)) {}
	}

	/*public interface IStores<T> : ISelect<ISelect<T[], ArrayView<T>>, ISelect<IEnumerable<T>, ArrayView<T>>> {}

	public readonly struct Storage<T>
	{
		public Storage(IStores<T> stores, Complete<T> complete = null)
		{
			Stores   = stores;
			Complete = complete;
		}

		public IStores<T> Stores { get; }

		public Complete<T> Complete { get; }
	}

	sealed class ArrayStores<T> : IStores<T>
	{
		public static ArrayStores<T> Default { get; } = new ArrayStores<T>();

		ArrayStores() : this(In<IEnumerable<T>>.Select(x => (T[])x)) {}

		readonly ISelect<IEnumerable<T>, T[]> _select;

		public ArrayStores(ISelect<IEnumerable<T>, T[]> select) => _select = @select;

		public ISelect<IEnumerable<T>, ArrayView<T>> Get(ISelect<T[], ArrayView<T>> parameter)
			=> _select.Select(parameter);
	}*/

	/*sealed class Fill<T> : ISelect<ICollection<T>, ArrayView<T>>
	{
		public static Fill<T> Default { get; } = new Fill<T>();

		Fill() : this(Lease<T>.Default) {}

		readonly ILease<T> _lease;

		public Fill(ILease<T> lease) => _lease = lease;

		public ArrayView<T> Get(ICollection<T> parameter)
		{
			var result = _lease.Get(parameter.Count);
			parameter.CopyTo(result.Array, 0);
			return result;
		}
	}

	sealed class Iterate<T> : ISelect<IEnumerable<T>, ArrayView<T>>
	{
		public static Iterate<T> Default { get; } = new Iterate<T>();

		Iterate() : this(Enumerate<T>.Default) {}

		readonly IEnumerate<T> _enumerate;

		public Iterate(IEnumerate<T> enumerate) => _enumerate = enumerate;

		public ArrayView<T> Get(IEnumerable<T> parameter) => _enumerate.Get(parameter.GetEnumerator());
	}*/

	public interface IDefinitionComposer<T> : ISelect<Definition<T>, ISelect<IEnumerable<T>, T[]>> {}

	sealed class DefinitionComposer<T> : IDefinitionComposer<T>
	{
		public static DefinitionComposer<T> Default { get; } = new DefinitionComposer<T>();

		DefinitionComposer() : this(BodyComposer<T>.Default) {}

		readonly IBodyComposer<T> _body;

		public DefinitionComposer(IBodyComposer<T> body) => _body = body;

		public ISelect<IEnumerable<T>, T[]> Get(Definition<T> parameter)
		{
			var body = parameter.Enter.Select.Select(_body.Get(parameter.Body));
			var contents = parameter.Enter.Complete != null
				               ? body.Select(new Completed<T>(parameter.Enter.Complete))
				               : body;
			var result = contents.Select(parameter.Result);
			return result;
		}
	}

	sealed class Compose<_, T> : ISelect<Composition<_, T>, ISelect<_, T[]>>
	{
		public static Compose<_, T> Default { get; } = new Compose<_, T>();

		Compose() : this(DefinitionComposer<T>.Default) {}

		readonly IDefinitionComposer<T> _composer;

		public Compose(IDefinitionComposer<T> composer) => _composer = composer;

		public ISelect<_, T[]> Get(Composition<_, T> parameter)
			=> parameter.Enter.Select(_composer.Get(parameter.Definition));
	}

	public delegate void Complete<in T>(T[] resource);

	public interface IDefinitionAlteration<T> : IAlteration<Definition<T>> {}

	sealed class Skip<T> : IContentAlteration<T>
	{
		readonly uint _skip;

		public Skip(uint skip) => _skip = skip;

		public Content<T> Get(Content<T> parameter)
		{
			var current = parameter.Selection ?? Selection.Default;
			var result = new Content<T>(parameter.Select, new Selection(current.Start + _skip,
			                                                            current.Length - _skip));
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
			var result = new Content<T>(parameter.Select, new Selection(current.Start, _take));
			return result;
		}
	}

	sealed class WhereDefinition<T> : DefinitionAlteration<T>
	{
		public WhereDefinition(Func<T, bool> where)
			: base(AllottedCopy<T>.Default, new WhereSegment<T>(where), Allotted<T>.Default.Execute) {}
	}

	class DefinitionAlteration<T> : IDefinitionAlteration<T>
	{
		readonly IBodyAlteration<T> _alteration;

		public DefinitionAlteration(ISegment<T> select, Complete<T> exit = null)
			: this(SelectView<T>.Default, @select, exit) {}

		public DefinitionAlteration(ISelect<T[], ArrayView<T>> enter, ISegment<T> select, Complete<T> exit = null)
			: this(new BodyContentAlteration<T>(new SegmentAlteration<T>(@select), enter, exit)) {}

		public DefinitionAlteration(IBodyAlteration<T> alteration) => _alteration = alteration;

		public Definition<T> Get(Definition<T> parameter)
			=> new Definition<T>(parameter.Enter, _alteration.Get(parameter.Body), parameter.Result);
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
		readonly IContentAlteration<T>      _content;
		readonly ISelect<T[], ArrayView<T>> _enter;
		readonly Complete<T>                _exit;

		public BodyContentAlteration(IContentAlteration<T> content, ISelect<T[], ArrayView<T>> enter = null,
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