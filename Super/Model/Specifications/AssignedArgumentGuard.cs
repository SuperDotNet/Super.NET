﻿using System;
using Super.ExtensionMethods;

namespace Super.Model.Specifications
{
	sealed class AssignedArgumentGuard<T> : GuardedSpecification<T, ArgumentNullException>
	{
		public static AssignedArgumentGuard<T> Default { get; } = new AssignedArgumentGuard<T>();

		AssignedArgumentGuard() : this(AssignedSpecification<T>.Default,
		                               new ArgumentNullException($"Argument of type {typeof(T)} was not assigned.")) {}

		public AssignedArgumentGuard(ISpecification<T> specification, ArgumentNullException exception)
			: base(specification, exception.Accept) {}
	}
}