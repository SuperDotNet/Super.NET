﻿using FluentAssertions;
using Super.Operations;
using System.Threading.Tasks;
using Xunit;

namespace Super.Testing.Application.Operations
{
	public class OperationTests
	{
		[Fact]
		Task Verify() => Number.Default.Then()
		                       .Then(x => x * 2)
		                       .Get()
		                       .Get("Hello World!")
		                       .ContinueWith(x => x.Result.Should().Be(24));

		sealed class Number : IOperation<string, uint>
		{
			public static Number Default { get; } = new Number();

			Number() {}

			public ValueTask<uint> Get(string parameter) => new ValueTask<uint>((uint)parameter.Length);
		}
	}
}