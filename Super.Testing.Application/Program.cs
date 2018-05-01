namespace Super.Testing.Application
{
	sealed class Program
	{
		static void Main()
		{
			/*BenchmarkRunner.Run<Decorations>();*/
		}
	}

	/*public class Decorations
	{
		readonly ICommand _action;
		readonly ICommand _decorated;

		public Decorations() : this(new ThrowCommand()) {}

		public Decorations(ICommand action) : this(action, Alteration.Default.Get(action)) {}

		public Decorations(ICommand action, ICommand decorated)
		{
			_action    = action;
			_decorated = decorated;
		}

		[Benchmark]
		public void Pure()
		{
			_action.Execute();
		}

		[Benchmark]
		public void Decorated()
		{
			_decorated.Execute();
		}

		sealed class Alteration : IAlteration<ICommand>
		{
			public static Alteration Default { get; } = new Alteration();

			Alteration() {}

			public ICommand Get(ICommand parameter)
			{
				return Enumerable.Range(0, 10)
				                 .Aggregate(parameter, (command, _) => new DecoratedCommand(command));
			}

			ICommand Alter(ICommand current) => new DecoratedCommand(current);
		}
	}*/

	/*public interface ICommand
	{
		void Execute();
	}

	sealed class DecoratedCommand : ICommand
	{
		readonly ICommand _command;

		public DecoratedCommand(ICommand command) => _command = command;

		public void Execute()
		{
			_command.Execute();
		}
	}

	sealed class ThrowCommand : ICommand
	{
		public void Execute()
		{
			Command.methodA();
		}
	}

	sealed class Command
	{
		public static Action<string> Call { get; set; } = s => {};

		public static void methodA() {
			methodB(); }

		static void methodB() {
			methodC(); }

		static void methodC() {
			badMethod(); }

		static void badMethod()
		{
			Call("Hello World!");
		}
	}*/
}
