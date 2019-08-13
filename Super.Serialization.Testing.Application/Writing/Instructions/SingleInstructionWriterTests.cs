using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Model.Sequences;
using Super.Serialization.Writing.Instructions;
using System.Text.Json;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public sealed class SingleInstructionWriterTests
	{
		[Fact]
		void Verify()
		{
			const uint number = 12345;
			Writer.Default.Get(number)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.SerializeToUtf8Bytes(number));
		}

		sealed class Writer : SingleInstructionWriter<uint>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(IntegerInstruction.Default) {}
		}

		public class Benchmarks
		{
			readonly IWriter<uint> _subject, _compare;
			readonly uint          _data;

			public Benchmarks() : this(Writer.Default, new Writer<uint>(IntegerInstruction.Default), 12345) {}

			public Benchmarks(IWriter<uint> subject, IWriter<uint> compare, uint data)
			{
				_subject = subject;
				_compare = compare;
				_data    = data;
			}

			[Benchmark(Baseline = true)]
			public Array<byte> Subject() => _subject.Get(_data);

			[Benchmark]
			public Array<byte> Compare() => _compare.Get(_data);
		}
	}
}