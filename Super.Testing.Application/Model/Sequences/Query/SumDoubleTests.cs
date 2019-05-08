﻿using FluentAssertions;
using Super.Compose;
using Super.Testing.Objects;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences.Query
{
	public sealed class SumDoubleTests
	{
		const uint Total = 1000;

		readonly static string[] Source = Data.Default.Get().Take((int)Total).ToArray();

		[Fact]
		void Verify()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Sum(x => (double)x.Length)
			     .Get(Source)
			     .Should()
			     .Be(Source.Sum(x => (double)x.Length));
		}

		[Fact]
		void VerifySelect()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Select(x => (double)x.Length)
			     .Sum()
			     .Get(Source)
			     .Should()
			     .Be(Source.Sum(x => (double)x.Length));
		}
	}
}