using JetBrains.Annotations;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sequences
{
	/*public interface IAlterState<T> : IAlteration<T> {}*/

	public interface IState<T> : IActivateMarker<T>, ISelect<IAlteration<T>, IState<T>>, ISource<T> {}

	public interface ISelectionState : IState<Collections.Selection> {}

	sealed class SelectionState : State<Collections.Selection>, ISelectionState
	{
		public static SelectionState Default { get; } = new SelectionState();

		SelectionState() : this(Collections.Selection.Default) {}

		public SelectionState(Collections.Selection instance) : base(instance, I<SelectionState>.Default.From) {}
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

	class StateBuilder<T, TResult> : Select<IAlteration<T>, TResult> //where TResult : IActivateMarker<IState<T>>
	{
		public StateBuilder(Func<IAlteration<T>, TResult> build) : base(build) {}
	}

	public interface INode<T> : ISource<IArraySelector<T>>,
	                            IActivateMarker<IState<Collections.Selection>>,
	                            IAlteration<ISegment<T>>, ISelect<IAlterSelection, INode<T>> {}

	sealed class Node<T> : Source<IArraySelector<T>>, INode<T>
	{
		public static Node<T> Default { get; } = new Node<T>();

		Node() : this(SelectionState.Default, SelectedSegment<T>.Default, ArraySelector<T>.Default) {}

		readonly IState<Collections.Selection> _state;
		readonly ISegment<T> _segment;

		[UsedImplicitly]
		public Node(IState<Collections.Selection> state) : this(state, state.Get()) {}

		public Node(IState<Collections.Selection> state, Collections.Selection selection)
			: this(state, new SelectedSegment<T>(selection), new ArraySelector<T>(selection)) {}

		public Node(IState<Collections.Selection> state, ISegment<T> segment, IArraySelector<T> instance) : base(instance)
		{
			_state = state;
			_segment = segment;
		}

		public ISegment<T> Get(ISegment<T> parameter) => _segment.Select(Sessions<T>.Default).Continue(parameter);

		public INode<T> Get(IAlterSelection parameter) => _state.Get(parameter).To(I<Node<T>>.Default.From);
	}

	/*public interface ISelections<T> : ISelect<IAlteration<Collections.Selection>, INode<T>> {}

	sealed class Selections<T> : StateBuilder<Collections.Selection, INode<T>>,
	                             ISelections<T>,
	                             IActivateMarker<ISelectionState>
	{
		public static Selections<T> Default { get; } = new Selections<T>();

		Selections() : this(SelectionState.Default) {}

		public Selections(ISelectionState state) : base(state.Select(I<Node<T>>.Default.From).Get) {}
	}*/
}