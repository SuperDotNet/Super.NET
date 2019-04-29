using FluentAssertions;
using Super.Aspects;
using Super.Compose;
using Super.Reflection.Types;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Super.Testing.Application.Aspects
{
	public class AssignedAspectTests
	{
		[Fact]
		void Verify()
		{
			var subject = Start.A.Selection<string>().By.Self;
			subject.Invoking(x => x.Get(null)).Should().NotThrow();
			subject.Configured()
			       .Invoking(x => x.Get(null))
			       .Should()
			       .Throw<InvalidOperationException>();
		}

		readonly ITestOutputHelper _output;

		public AssignedAspectTests(ITestOutputHelper output) => _output = output;

		[Fact]
		void Type()
		{
			/*var subject = Start.A.Selection<string>().By.Self;

			var single = new RuntimeRegistration<string, string>(typeof(AssignedAspect<,>));
			single.Get(subject).Should().BeSameAs(AssignedAspect<string, string>.Default);*/
			var yo = GenericInterfaceImplementations.Default.Get(typeof(AssignedAspect<,>))
			                               .Condition.Get(typeof(IAspect<,>));

			_output.WriteLine(yo.ToString());
		}

		/*public sealed class IsAssignableFromOpenGeneric : ICondition<Type>
		{
			readonly Type _definition;

			public IsAssignableFromOpenGeneric(Type definition) => _definition = definition;

			public bool Get(Type parameter)
			{
				var interfaceTypes = parameter.GetInterfaces();

				foreach (var it in interfaceTypes)
				{
					if (it.IsGenericType && it.GetGenericTypeDefinition() == _definition)
					{
						return true;
					}
				}

				if (parameter.IsGenericType && parameter.GetGenericTypeDefinition() == _definition)
				{
					return true;
				}

				var baseType = parameter.BaseType;

				if (baseType == null)
				{
					return false;
				}

				return Get(baseType);
			}
		}*/

		/*[Fact]
		void Count()
		{
			var first = 0;
			Enumerable.Range(0, 100)
			          .Select(x =>
			                  {
				                  first++;
				                  return x;
			                  })
			          .Select(x =>
			                  {
				                  first++;
				                  return x;
			                  })
			          .FirstOrDefault();

			first.Should().Be(2);

			var second = 0;

			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Select(x =>
			             {
				             second++;
				             return x;
			             })
			     .Select(x =>
			             {
				             second++;
				             return x;
			             })
			     .First()
			     .Get(Data.Default.Get());

			second.Should().Be(2);
		}*/
	}
}