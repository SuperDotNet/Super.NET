using Super.Compose;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Text;
using System;

namespace Super.Runtime
{
	sealed class DefaultGuard<T> : AssignedGuard<T>
	{
		public static DefaultGuard<T> Default { get; } = new DefaultGuard<T>();

		DefaultGuard() : base(DefaultMessage.Default.Get) {}
	}

	public class AssignedGuard<T> : AssignedGuard<T, InvalidOperationException>
	{
		protected AssignedGuard(Func<Type, string> message) : base(message) {}

		public AssignedGuard(ISelect<T, string> message) : base(message) {}
	}

	public class AssignedGuard<T, TException> : Guard<T, TException> where TException : Exception
	{
		readonly static ICondition<T> Condition = IsAssigned<T>.Default.Then().Inverse().Out();

		protected AssignedGuard(Func<Type, string> message) : this(message.ToSelect()
		                                                                  .In(A.Type<T>())
		                                                                  .ToSelect(I.A<T>())) {}

		public AssignedGuard(ISelect<T, string> message) : base(Condition, message) {}
	}

	sealed class AssignedArgumentGuard<T> : AssignedGuard<T, ArgumentNullException>
	{
		public static AssignedArgumentGuard<T> Default { get; } = new AssignedArgumentGuard<T>();

		AssignedArgumentGuard() : base(x => $"Argument of type {x} was not assigned.") {}
	}

	public sealed class AssignableFromGuard : Guard<Type, InvalidOperationException>
	{
		public AssignableFromGuard(Type type) : this(type, new Message(type)) {}

		public AssignableFromGuard(Type type, ISelect<Type, string> message)
			: this(new IsAssignableFrom(type).Then().Inverse().Out(), message) {}

		public AssignableFromGuard(ICondition<Type> condition, ISelect<Type, string> message)
			: base(condition, message) {}

		sealed class Message : IFormatter<Type>
		{
			readonly Type _expected;

			public Message(Type expected) => _expected = expected;

			public string Get(Type parameter) => $"'{parameter}' is not of type '{_expected}'.";
		}
	}

	public sealed class ContainsGenericInterfaceGuard : Guard<Type, InvalidOperationException>
	{
		public ContainsGenericInterfaceGuard(Type type) : this(type, new Message(type)) {}

		public ContainsGenericInterfaceGuard(Type type, ISelect<Type, string> message)
			: this(new ContainsGenericInterface(type).Then().Inverse().Out().Get, message) {}

		public ContainsGenericInterfaceGuard(Func<Type, bool> condition, ISelect<Type, string> message)
			: base(condition, message) {}

		sealed class Message : IFormatter<Type>
		{
			readonly Type _expected;

			public Message(Type expected) => _expected = expected;

			public string Get(Type parameter) => $"'{parameter}' is not of type '{_expected}'.";
		}
	}

	public class Guard<T, TException> : ICommand<T> where TException : Exception
	{
		readonly Func<T, TException> _exception;
		readonly Func<T, bool>       _condition;

		public Guard(ICondition<T> condition, ISelect<T, string> message) : this(condition.Get, message) {}

		public Guard(Func<T, bool> condition, ISelect<T, string> message)
			: this(condition, Start.A.Selection<string>()
			                       .AndOf<TException>()
			                       .By.Instantiation.To(message.Select)
			                       .Get) {}

		public Guard(Func<T, bool> condition, Func<T, TException> exception)
		{
			_condition = condition;
			_exception = exception;
		}

		public void Execute(T parameter)
		{
			if (_condition(parameter))
			{
				throw _exception(parameter);
			}
		}
	}
}