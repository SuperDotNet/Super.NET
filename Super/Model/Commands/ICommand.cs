namespace Super.Model.Commands
{
	public interface ICommand<in T>
	{
		void Execute(T parameter);
	}
}