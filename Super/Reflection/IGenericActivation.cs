using System.Linq.Expressions;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	public interface IGenericActivation : ISource<TypeInfo, Expression> {}
}