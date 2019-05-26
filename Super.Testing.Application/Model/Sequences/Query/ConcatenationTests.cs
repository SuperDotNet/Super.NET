﻿using FluentAssertions;
using Super.Compose;
using Super.Model.Sequences;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences.Query
{
	public sealed class ConcatenationTests
	{
		const int skip = 10;

		readonly static int[] data =
		{
			0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8,
			8, 9, 9, 9, 9, 9, 9, 9, 9, 9
		};

		[Fact]
		void Verify()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Append(Sequence.From(1, 2, 3))
			     .Get(data)
			     .Open()
			     .Should()
			     .Equal(data.Concat(new[] {1, 2, 3}));
		}

		[Fact]
		void VerifyBody()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(skip)
			     .Append(Sequence.From(1, 2, 3))
			     .Get(data)
			     .Open()
			     .Should()
			     .Equal(data.Skip(skip).Concat(new[] {1, 2, 3}));
		}

		[Fact]
		void VerifyBodyFirst()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(skip)
			     .Append(Sequence.From(1, 2, 3))
			     .Skip(skip)
			     .First()
			     .Get(data)
			     .Should()
			     .Be(data.Skip(skip).Concat(new[] {1, 2, 3}).Skip(skip).First());

			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(skip)
			     .Append(Sequence.From(1, 2, 3))
			     .First()
			     .Get(data)
			     .Should()
			     .Be(data.Skip(skip).Concat(new[] {1, 2, 3}).First());
		}
	}
}