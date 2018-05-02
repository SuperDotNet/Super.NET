using AutoFixture;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Super.Model.Collections;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Testing.Application
{
	sealed class Program
	{
		static void Main()
		{
			BenchmarkRunner.Run<Arrays>();
		}
	}

	[MemoryDiagnoser]
	public class Arrays
	{
		readonly Func<string, int> _select;
		readonly Array<string> _array;
		readonly string[] _data;
		readonly ImmutableArray<string> _immutable;

		readonly IArray<string, int> _selector;
		readonly IArray<string, int> _where;
		readonly IArray<string, int> _inline;


		public Arrays() : this(x => x.Length, new Fixture().CreateMany<string>(10_000).ToArray()) {}

		public Arrays(Expression<Func<string, int>> expression, string[] data)
			: this(expression, new Array<string>(data), data, data.ToImmutableArray()) {}

		public Arrays(Expression<Func<string, int>> expression, Array<string> array, string[] data, ImmutableArray<string> immutable)
			: this(expression, expression.Compile(), array, data, immutable) {}

		public Arrays(Expression<Func<string, int>> expression, Func<string, int> select, Array<string> array, string[] data, ImmutableArray<string> immutable)
		{
			_select = select;
			_array = array;
			_data = data;
			_immutable = immutable ;
			_selector = new ArraySelect<string, int>(In<string>.Select(_select));
			_where = new ArrayWhere<string, int>(_select, x => x > 10);
			_inline = new ArraySelectInline<string, int>(expression);
		}

		[Benchmark]
		public int[] Native() => _data.Select(_select)./*Where(x => x > 10).*/ToArray();

		/*[Benchmark]
		public Array<int> Select() => _selector.Get(_array);*/

		/*[Benchmark]
		public Array<int> Range() => _range.Get(_array);*/

		[Benchmark]
		public Array<int> Inline() => _inline.Get(_array);

		/*

		[Benchmark]
		public Array<int> CustomConverter() => _converter.Get(_array);

		[Benchmark]
		public Array<int> CustomWhere() => _where.Get(_array);*/
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
