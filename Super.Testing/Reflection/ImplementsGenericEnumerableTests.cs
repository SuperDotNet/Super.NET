using FluentAssertions;
using Super.Model.Commands;
using Super.Model.Sources;
using Super.Reflection.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Xunit;

namespace Super.Testing.Reflection
{
	public class ImplementsGenericEnumerableTests
	{
		sealed class Source : ISource<object>
		{
			public object Get() => throw new NotSupportedException();
		}

		sealed class Command : ICommand<Unit>
		{
			public void Execute(Unit parameter)
			{
				throw new NotSupportedException();
			}
		}

		sealed class Subject : IEnumerable<object>
		{
			public IEnumerator<object> GetEnumerator() => throw new NotSupportedException();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		sealed class Subject<T> : IEnumerable<T>
		{
			public IEnumerator<T> GetEnumerator() => throw new NotSupportedException();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		sealed class Integers : IEnumerable<int>
		{
			public IEnumerator<int> GetEnumerator() => throw new NotSupportedException();

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
				typeof(Source),
				typeof(Command)
			}.ToMetadata()
			 .All(ImplementsGenericEnumerable.Default.IsSatisfiedBy)
			 .Should()
			 .BeFalse();
	}
}