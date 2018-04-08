using System;
using System.Linq.Expressions;
using System.Reflection;
using Super.Runtime.Invocation.Expressions;
using Xunit;

namespace Super.Testing.Expressions
{
	public class ExpressionMemberNameTests
	{
		static MemberInfo Check<T1>(Expression<Func<T1, object>> expression) => expression.GetMemberInfo();

		class Subject
		{
			public Subject(object parameter) => Parameter = parameter;

			public object Parameter { get; }
		}

		class Structure
		{
			public Structure(int parameter) => Parameter = parameter;

			public int Parameter { get; }
		}

		[Fact]
		public void GetMemberInfo()
		{
			var info = Check<Subject>(parameter => parameter.Parameter);
			Assert.Equal(nameof(Subject.Parameter), info.Name);
		}

		[Fact]
		public void GetMemberInfoStructure()
		{
			var info = Check<Structure>(parameter => parameter.Parameter);
			Assert.Equal(nameof(Structure.Parameter), info.Name);
		}
	}
}