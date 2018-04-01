using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using FluentAssertions;
using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Instances;
using Super.Reflection;
using Xunit;

namespace Super.Testing.Reflection
{
	public class ImplementsGenericEnumerableTests
	{
		sealed class Instance : IInstance<object>
		{
			public object Get() => throw new NotImplementedException();
		}

		sealed class Command : ICommand<Unit>
		{
			public void Execute(Unit parameter)
			{
				throw new NotImplementedException();
			}
		}

		sealed class Subject : IEnumerable<object>
		{
			public IEnumerator<object> GetEnumerator() => throw new NotImplementedException();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		sealed class Subject<T> : IEnumerable<T>
		{
			public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		sealed class Integers : IEnumerable<int>
		{
			public IEnumerator<int> GetEnumerator() => throw new NotImplementedException();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		[Fact]
		public void Are() => new[]
			{
				typeof(Subject),
				typeof(Subject<int>),
				typeof(Integers)
			}.ToMetadata()
			 .All(ImplementsGenericEnumerable.Default.IsSatisfiedBy)
			 .Should()
			 .BeTrue();

		[Fact]
		public void AreNot() => new[]
			{
				typeof(Instance),
				typeof(Command)
			}.ToMetadata()
			 .All(ImplementsGenericEnumerable.Default.IsSatisfiedBy)
			 .Should()
			 .BeFalse();
	}
}