using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Structure;
using Super.Model.Specifications;
using Super.Runtime.Activation;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Super.Model.Sequences
{
	public interface IStore<T> : ISelect<uint, T[]>, ICommand<T[]> {}

	sealed class Allocated<T> : DelegatedCommand<T[]>, IStore<T>
	{
		public static Allocated<T> Default { get; } = new Allocated<T>();

		Allocated() : base(Runtime.Delegates<T[]>.Empty) {}

		public T[] Get(uint parameter) => new T[parameter];
	}

	sealed class Allotted<T> : IStore<T>
	{
		public static Allotted<T> Default { get; } = new Allotted<T>();

		Allotted() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Allotted(ArrayPool<T> pool) => _pool = pool;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(uint parameter) => _pool.Rent((int)parameter);

		public void Execute(T[] parameter)
		{
			_pool.Return(parameter);
		}
	}

	/*sealed class CopySelection<T> : ISelection<T>
	{
		public static CopySelection<T> Default { get; } = new CopySelection<T>();

		CopySelection() : this(Allocated<T>.Default) {}

		readonly IStore<T>     _store;
		readonly IStoreCopy<T> _clone;

		public CopySelection(IStore<T> store) : this(store, new StoreCopy<T>(store)) {}

		public CopySelection(IStore<T> store, IStoreCopy<T> clone)
		{
			_store = store;
			_clone = clone;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Session<T> Get(in ArrayView<T> parameter) => _store.Session(_clone.Get(parameter));
	}*/

	/*sealed class CopySelection<T> : ISelection<T>
	{
		public static CopySelection<T> Default { get; } = new CopySelection<T>();

		CopySelection() : this(Allocated<T>.Default) {}

		readonly IStore<T>     _store;
		readonly IStoreCopy<T> _clone;

		public CopySelection(IStore<T> store) : this(store, new StoreCopy<T>(store)) {}

		public CopySelection(IStore<T> store, IStoreCopy<T> clone)
		{
			_store = store;
			_clone = clone;
		}

		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ILocal<Session<T>> Get(ILocal<ArrayView<T>> parameter)
			=> Locals.For(_store.Session(_clone.Get(parameter.Get())));
	}*/

	public interface ISelector<T> : IStructure<ArrayView<T>>, IActivateMarker<IStore<T>> {}

	sealed class Selector<T> : ISelector<T>
	{
		public static Selector<T> Default { get; } = new Selector<T>();

		Selector() {}

		public ArrayView<T> Get(in ArrayView<T> parameter) => parameter;
	}

	public interface INodeAlteration<T> : ISpecification<INodeAlteration<T>>, IAlteration<INodeBuilder<T>> {}

	public interface INodeBuilder<T> : ISelect<Definition<T>, ISelect<T[], T[]>> {}

	class NodeBuilder<T> : INodeBuilder<T>
	{
		public static NodeBuilder<T> Default { get; } = new NodeBuilder<T>();

		NodeBuilder() {}

		public ISelect<T[], T[]> Get(Definition<T> parameter) => null;
	}

	public interface INode<T> : IStructure<ArrayView<T>, T[]> {}

	/*sealed class Sessions<T> : ISelection<T>
	{
		public static Sessions<T> Default { get; } = new Sessions<T>();

		Sessions() : this(Allocated<T>.Default, Locals<Session<T>>.Default) {}

		readonly IStore<T>           _store;
		readonly ILocals<Session<T>> _locals;

		public Sessions(IStore<T> store, ILocals<Session<T>> locals)
		{
			_store  = store;
			_locals = locals;
			var session = default(Session<T>);
			_locals.Get(ref session);
		}

		 [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ILocal<Session<T>> Get(ILocal<ArrayView<T>> parameter)
		{
			var view    = parameter.Get();
			var session = new Session<T>(_store.Get(view.Length), _store, view.Length);
			var result  = _locals.Get(ref session);
			return result;
		}
	}*/

	/*sealed class Copy<T> : ISelection<T>
	{
		public static Copy<T> Default { get; } = new Copy<T>();

		Copy() : this(Allocated<T>.Default, null) {}

		readonly IStore<T> _store;
		readonly ISelection<T> _selection;

		public Copy(IStore<T> store, ISelection<T> selection)
		{
			_store = store;
			_selection = selection;
		}

		// [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Session<T> Get(in ArrayView<T> parameter) => _store.Session(parameter.Length);
	}*/

	/*sealed class DefaultNode<T> : INode<T>
	{
		public static DefaultNode<T> Default { get; } = new DefaultNode<T>();

		DefaultNode() : this(Copy<T>.Default.Get, Selection.Structure.Self<ArrayView<T>>.Default) {}

		readonly Func<uint, Session<T>> _session;
		readonly IStructure<ArrayView<T>> _select;

		public DefaultNode(Func<uint, Session<T>> session, IStructure<ArrayView<T>> select)
		{
			_session = session;
			_select = @select;
		}

		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(in ArrayView<T> parameter)
		{
			using (var session = Collections.Allocated<T>.Default.Session(parameter.Length))
			{
				return parameter.Array.CopyInto(session.Array, parameter.Start, session.Length)/*_select.Get(session)#1#;
			}




			/*using (var select = _store.Get(parameter))
			{
				return /*_select.Get(new ArrayView<T>(select.Array, 0, select.Length)).Array#2#select.Array;
			}#1#
			/*var session = new Session<T>(_store.Get(parameter.Length), _store);
			using (var disposable = session)
			{
				//using (var context = /*Sessions<T>.Default.Get(new ArrayView<T>(parameter, _size.Start, _size.Length ?? (uint)parameter.Length))#2#disposable)
				var selection = new Collections.Selection(parameter.Start, parameter.Length);
				return parameter.Array.Copy(disposable.Array, in selection);
			}#1#
		}
	}*/

	public sealed class Definition<T>
	{
		public Definition() : this(Allocated<T>.Default, Selector<T>.Default,
		                           new KeyedByTypeCollection<IAlterSelection<T>>()) {}

		public Definition(IStore<T> store, ISelector<T> selection, IList<IAlterSelection<T>> alterations)
		{
			Store       = store;
			Selection   = selection;
			Alterations = alterations;
		}

		public IStore<T> Store { get; }

		public ISelector<T> Selection { get; }

		public IList<IAlterSelection<T>> Alterations { get; }
	}

	sealed class DefaultArraySelection<T> : IAlteration<T[]>
	{
		public static DefaultArraySelection<T> Default { get; } = new DefaultArraySelection<T>();

		DefaultArraySelection() : this(Collections.Selection.Default) {}

		readonly Func<uint, T[]> _source;
		readonly Collections.Selection _selection;

		public DefaultArraySelection(Collections.Selection selection) : this(Allocated<T>.Default.Get, selection) {}

		public DefaultArraySelection(Func<uint, T[]> source, Collections.Selection selection)
		{
			_source = source;
			_selection = selection;
		}

		public T[] Get(T[] parameter)
		{
			var size = _selection.Length.IsAssigned ? _selection.Length.Instance : (uint)parameter.Length;
			return parameter.CopyInto(_source(size), _selection.Start, size);
		}
	}
}