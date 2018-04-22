using System.Reflection;
using AutoFixture.Kernel;

namespace Super.Application.Hosting.xUnit
{
	public class ParameterRequestRelay : ISpecimenBuilder
	{
		readonly static NoSpecimen NoSpecimen = new NoSpecimen();

		readonly AutoFixture.Kernel.ParameterRequestRelay _inner;

		public ParameterRequestRelay(AutoFixture.Kernel.ParameterRequestRelay inner) => _inner = inner;

		public object Create(object request, ISpecimenContext context)
		{
			var result = request is ParameterInfo parameter
				             ? (ShouldDefault(parameter) ? parameter.DefaultValue : _inner.Create(request, context))
				             : NoSpecimen;
			return result;
		}

		static bool ShouldDefault(ParameterInfo info) => info.IsOptional
		//&& !CurrentMethod.Default.Get().GetParameterTypes().Any(info.ParameterType.IsAssignableFrom)
		;
	}
}