﻿using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Runtime;
using System;

namespace Super.Model.Specifications
{
	sealed class AssignedInstanceGuard<T> : GuardedSpecification<T, InvalidOperationException>
	{
		public AssignedInstanceGuard(ISpecification<T> specification, IMessage<T> message)
			: this(specification,
			       Exception<InvalidOperationException>.Default.In(message).Get) {}

		public AssignedInstanceGuard(ISpecification<T> specification, Func<T, InvalidOperationException> exception) :
			base(specification, exception) {}
	}
}