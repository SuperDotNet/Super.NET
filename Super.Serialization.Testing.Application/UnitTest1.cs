using BenchmarkDotNet.Attributes;
using FluentAssertions;
using System;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			new Write().Should().NotBeNull();
		}

		public class Benchmarks
		{
			int[] _subject;

			public Benchmarks() : this(new int[5]) {}

			public Benchmarks(int[] subject)
			{
				_subject = subject;
			}

			[Benchmark]
			public Array Resize()
			{
				Array.Resize(ref _subject, 10);
				return _subject;
			}

			/*[Benchmark]
			public Subject New() => new Subject { Message = "Hello World!", Number = 1234 };

			[Benchmark]
			public object Read() => JsonSerializer.Parse<Subject>(@"{""Message"":""Hello World!"", ""Number"" : 1234}");

			public class Subject
			{
				public string Message { get; set; }

				public int Number { get; set; }
			}*/
		}
	}
}
