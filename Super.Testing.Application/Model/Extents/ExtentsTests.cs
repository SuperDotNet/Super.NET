using FluentAssertions;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection.Types;
using System;
using System.Reflection;
using Xunit;

namespace Super.Testing.Application.Model.Extents
{
	public sealed class ExtentsTests
	{
		[Fact]
		void VerifySourceDirect()
		{
			new Source<string>("Hello World!").Out(x => x.Type().Metadata().Out())
			                                  .Get()
			                                  .Should()
			                                  .Be(Type<string>.Metadata);
		}

		[Fact]
		void VerifySourceDelegated()
		{
			6776.Start().Select(x => x.GetType().GetTypeInfo()).Get().Should().Be(Type<int>.Metadata);
		}

		[Fact]
		void VerifyGuard()
		{
			Start.From<string>().Self().Guard().Invoking(x => x.Get(null)).Should().Throw<InvalidOperationException>();
			Start.From<string>().Self().Invoking(x => x.Get(null)).Should().NotThrow();
		}

		[Fact]
		void VerifyOnlyOnce()
		{
			var count   = 0;
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