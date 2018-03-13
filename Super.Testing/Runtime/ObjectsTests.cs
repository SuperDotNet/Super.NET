using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Super.ExtensionMethods;
using Super.Runtime;
using Super.Runtime.Activation;
using Xunit;

namespace Super.Testing.Runtime
{
	public class ObjectsTests
	{
		[Fact]
		public void AsInvalid()
		{
			Assert.Throws<InvalidOperationException>(() => 2.To<string>());
		}

		[Theory, AutoData]
		void AsTo(string message)
		{
			1.AsTo<string, object>(x => x, () => message).Should().Be(message);
			1.AsTo<string, object>(x => x).Should().BeNull();
		}

		[Fact]
		public void With()
		{
			var count = 0;
			count.With(x =>
			           {
				           count++;
			           });
			count.Should()
			     .Be(1);
		}



		[Fact]
		public void ToDictionary()
		{
			var dictionary = new[] {Pairs.Create("Hello", "World")}.ToDictionary();
			dictionary["Hello"]
				.Should()
				.Be("World");
		}

		[Fact]
		public void GetValid()
		{
			Assert.Throws<InvalidOperationException>(() => Objects.To<int>(new ServiceProvider()));
		}

		[Fact]
		public void Accept()
		{
			var o = new object();
			o.Accept(123)
			 .Should()
			 .BeSameAs(o);
		}

		[Fact]
		public void Self()
		{
			var o = new object();
			o.Self()
			 .Should()
			 .BeSameAs(o);
		}

		[Fact]
		public void NullIfEmpty()
		{
			var message = "Hello World!";
			message.NullIfEmpty()
			       .Should()
			       .Be(message);
			string.Empty.NullIfEmpty()
			      .Should()
			      .BeNull();
		}

		[Fact]
		public void YieldMetadata()
		{
			GetType()
				.Yield()
				.YieldMetadata(x => x.Name != string.Empty)
				.Only()
				.Should()
				.Be(GetType());
		}

		[Theory, AutoData]
		public void AsToDefault(char[] expected)
		{

			3.AsTo<string, char[]>(x => x.ToCharArray(), expected.Self)
			 .Should()
			 .BeSameAs(expected);
		}

	}
}