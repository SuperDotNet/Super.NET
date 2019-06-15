using BenchmarkDotNet.Attributes;
using FluentAssertions;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public sealed class StagedWriterTests
	{
		[Fact]
		async Task Verify()
		{
			using (var stream = new MemoryStream())
			{
				await new SingleStagedWriter<uint>(PositiveNumber.Default).Get(new Input<uint>(stream, 12345u));
				stream.ToArray().Should().Equal(JsonSerializer.ToBytes(12345u));
			}
		}

		public class Benchmarks
		{
			readonly IStagedWriter<uint> _writer;
			readonly uint               _data;

			public Benchmarks() : this(new SingleStagedWriter<uint>(PositiveNumber.Default), 12345u) {}

			public Benchmarks(IStagedWriter<uint> writer, uint data)
			{
				_writer = writer;
				_data   = data;
			}

			[Benchmark(Baseline = true)]
			public async Task<byte[]> Native()
			{
				using (var stream = new MemoryStream())
				{
					await JsonSerializer.WriteAsync(_data, stream);
					return stream.ToArray();
				}
			}

			[Benchmark]
			public async Task<byte[]> Subject()
			{
				using (var stream = new MemoryStream())
				{
					await _writer.Get(new Input<uint>(stream, 12345u));
					return stream.ToArray();
				}
			}
		}
	}
}