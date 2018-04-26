using FluentAssertions;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection.Types;
using System;
using Xunit;

namespace Super.Testing.Application.Model.Extents
{
	public sealed class ExtentsTests
	{
		[Fact]
		void VerifySourceDirect()
		{
			new Source<string>("Hello World!").Select().Type().Metadata().Return().Get().Should().Be(Type<string>.Metadata);
		}

		[Fact]
		void VerifySourceDelegated()
		{
			new Source<int>(6776).Out(x => x.Type().Metadata()).Get().Should().Be(Type<int>.Metadata);
		}

		[Fact]
		void VerifyGuard()
		{
			In<string>.Start().Guard().Invoking(x => x.Get(null)).Should().Throw<InvalidOperationException>();
			In<string>.Start().Invoking(x => x.Get(null)).Should().NotThrow();
		}

		[Fact]
		void VerifyOnlyOnce()
		{
			var count = 0;
			var counter = new Select<string, int>(x => count++).OnlyOnce();
			count.Should().Be(0);
			counter.Get("HelloWorld");
			count.Should().Be(1);
			counter.Get("HelloWorld");
			count.Should().Be(1);
			counter.Get("HelloWorld 1");
			count.Should().Be(1);
			counter.Get("HelloWorld 2");
			count.Should().Be(1);
		}

		[Fact]
		void VerifyOnceStriped()
		{
			var count   = 0;
			var counter = new Select<string, int>(x => count++).OnceStriped();
			count.Should().Be(0);
			counter.Get("HelloWorld");
			count.Should().Be(1);
			counter.Get("HelloWorld");
			count.Should().Be(1);
			counter.Get("HelloWorld 1");
			count.Should().Be(2);
			counter.Get("HelloWorld 2");
			count.Should().Be(3);
			counter.Get("HelloWorld 2");
			count.Should().Be(3);
		}
	}
}