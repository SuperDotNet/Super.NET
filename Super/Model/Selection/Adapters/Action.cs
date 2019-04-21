using Super.Model.Commands;
using Super.Runtime;

namespace Super.Model.Selection.Adapters
{
	public interface IAction<in T> : ISelect<T, None> {}

	public class Action<T> : DelegatedCommand<T>, IAction<T>
	{
		public static implicit operator Action<T>(System.Action<T> value) => new Action<T>(value);

		public Action(System.Action<T> body) : base(body) {}

		public None Get(T parameter)
		{
			Execute(parameter);
			return None.Default;
		}
	}

	public class Action : DelegatedCommand, IAction<None>, ICommand
	{
		public static implicit operator Action(System.Action value) => new Action(value);

		public Action(System.Action action) : base(action) {}

		public None Get(None parameter)
		{
			Execute(parameter);
			return None.Default;
		}
	}
}