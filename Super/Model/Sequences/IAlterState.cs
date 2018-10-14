using JetBrains.Annotations;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sequences
{
	public interface IState<T> : IActivateMarker<T>, ISelect<IAlteration<T>, IState<T>>, ISource<T> {}

	public interface ISelectionState : IState<Selection> {}

	sealed class SelectionState : State<Selection>, ISelectionState
	{
		public static SelectionState Default { get; } = new SelectionState();

		SelectionState() : this(Selection.Default) {}

		public SelectionState(Selection instance) : base(instance, I<SelectionState>.Default.From) {}
	}

	class State<T> : IState<T>
	{
		readonly T                  _instance;
		readonly Func<T, IState<T>> _activate;

		public State(T instance, Func<T, IState<T>> activate)
		{
			_instance = instance;
			_activate = activate;
		}

		public IState<T> Get(IAlteration<T> parameter) => _activate(parameter.Get(_instance));

		public T Get() => _instance;
	}

	public interface INode<T> : ISource<IArraySelector<T>>,
	                            IActivateMarker<IState<Selection>>,
	                            IAlteration<ISelectView<T>>, ISelect<IAlterSelection, INode<T>> {}

	sealed class Node<T> : Source<IArraySelector<T>>, INode<T>
	{
		public static Node<T> Default { get; } = new Node<T>();

		Node() : this(SelectionState.Default, Selection<T>.Default, ArraySelector<T>.Default) {}

		readonly IState<Selection> _state;
		readonly ISelectView<T> _segment;

		[UsedImplicitly]
		public Node(IState<Selection> state) : this(state, state.Get()) {}

		public Node(IState<Selection> state, Selection selection)
			: this(state, new Selection<T>(selection), new ArraySelector<T>(selection)) {}

		public Node(IState<Selection> state, ISelectView<T> segment, IArraySelector<T> instance) : base(instance)
		{
			_state = state;
			_segment = segment;
		}

		public ISelectView<T> Get(ISelectView<T> parameter) => _segment.Select(Sessions<T>.Default).Continue(parameter);

		public INode<T> Get(IAlterSelection parameter) => _state.Get(parameter).To(I<Node<T>>.Default.From);
	}
}