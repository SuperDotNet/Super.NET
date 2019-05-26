using System;
using Super.Model.Selection;
using Super.Reflection.Types;
using Super.Text;

namespace Super.Runtime
{
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
}