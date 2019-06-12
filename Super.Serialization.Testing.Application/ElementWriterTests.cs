using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Model.Sequences;
using Super.Serialization.Xml;
using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public sealed class ElementWriterTests
	{
		[Fact]
		void Verify()
		{
			Subject.Default.Get(12345)
			       .Open()
			       .To(Encoder.Default.Get)
			       .Should()
			       .Be($@"<?xml version=""1.0""?>{Environment.NewLine}<unsignedInt>12345</unsignedInt>");
		}

		[Fact]
		void VerifyClassic()
		{
			var serializer = new XmlSerializer(typeof(uint));

			using (var stream = new MemoryStream())
			{
				serializer.Serialize(stream, 12345u);
				Subject.Default.Get(12345u).Open().Should().Equal(stream.ToArray());
			}
		}

		sealed class Subject : Writer<uint>
		{
			public static Subject Default { get; } = new Subject();

			Subject() : base(new XmlDocumentEmitter<uint>(new Emit(Declaration.Default),
			                                              new XmlElementWriter<uint>("unsignedInt", PositiveNumber.Default))) {}
		}

		public class Benchmarks
		{
			// ReSharper disable once NotAccessedField.Local
			readonly XmlSerializer _serializer;

			public Benchmarks() : this(new XmlSerializer(typeof(uint))) {}

			public Benchmarks(XmlSerializer serializer) => _serializer = serializer;

			/*[Benchmark(Baseline = true)]
			public byte[] Classic()
			{
				using (var stream = new MemoryStream())
				{
					_serializer.Serialize(stream, 12345u);
					return stream.ToArray();
				}
			}*/

			[Benchmark]
			public Array<byte> Measure() => Subject.Default.Get(12345);
		}
	}
}