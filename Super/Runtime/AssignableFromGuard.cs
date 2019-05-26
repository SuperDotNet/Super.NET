using System;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Reflection.Types;
using Super.Text;

namespace Super.Runtime
{
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
}