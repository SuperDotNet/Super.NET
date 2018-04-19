namespace Super.Model.Commands
{
	public class DecoratedCommand<T> : DelegatedCommand<T>
	{
		public DecoratedCommand(ICommand<T> command) : base(command.ToDelegate()) {}
	}
}