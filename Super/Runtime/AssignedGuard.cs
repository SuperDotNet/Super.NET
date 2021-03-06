﻿using System;
using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Reflection;

namespace Super.Runtime
{
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
}