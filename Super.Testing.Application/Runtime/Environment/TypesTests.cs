using FluentAssertions;
using Super.Model.Collections;
using Super.Runtime.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
// ReSharper disable All

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class TypesTests
	{
		[Fact]
		void Verify()
		{
			Super.Runtime.Environment.Types.Default.Execute(GetType().Yield().ToArray());

			Super.Runtime.Environment.Types.Default.Get().Should().HaveCount(1);
			Super.Runtime.Environment.Types.Default.Execute(default);

			Super.Runtime.Environment.Types.Default.Get().Should().HaveCountGreaterThan(1);
		}

		[Fact]
		void Count()
		{
			var counter = new Counter();
			var source = new TypeSource();
			counter.Get().Should().Be(0);
			var types = new Types(counter, source);
			counter.Get().Should().Be(1);
			Super.Runtime.Environment.Types.Default.Execute(types);
			counter.Get().Should().Be(1);
			Super.Runtime.Environment.Types.Default.Get().Should().BeEquivalentTo(source);
			counter.Get().Should().Be(1);
			Super.Runtime.Environment.Types.Default.Get().Should().BeEquivalentTo(source);

			Super.Runtime.Environment.Types.Default.Get().Should().Equal(Super.Runtime.Environment.Types.Default.Get());
			types.Get().Should().BeEquivalentTo(types.Get());
			counter.Get().Should().Be(1);
		}

		sealed class Types : ArrayInstance<Type>
		{
			public Types(Counter counter, IEnumerable<Type> types) : base(types.Select(counter.ExecuteAndReturn)) {}
		}

		sealed class TypeSource : Enumerable<Type>
		{
			public override IEnumerator<Type> GetEnumerator()
			{
				yield return typeof(object);
//				yield return typeof(int);
			}
		}
	}
}