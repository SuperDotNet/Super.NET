using Super.Model.Selection.Alterations;
using Super.Runtime.Activation;
using Super.Runtime.Execution;
using System.Reactive;

namespace Super.Model.Specifications
{
	public interface ISpecification : ISpecification<Unit> {}

	public interface ISpecification<in T>
	{
		bool IsSatisfiedBy(T parameter);
	}

	public interface IAny : ISpecification, ISpecification<object> {}

	sealed class Any : IAny, IActivateMarker<ISpecification>
	{
		readonly ISpecification _specification;

		public Any(ISpecification specification) => _specification = specification;

		public bool IsSatisfiedBy(object _) => _specification.IsSatisfiedBy();

		public bool IsSatisfiedBy(Unit _) => _specification.IsSatisfiedBy();
	}

	sealed class OnlyOnceAlteration<T> : IAlteration<ISpecification<T>>
	{
		public static OnlyOnceAlteration<T> Default { get; } = new OnlyOnceAlteration<T>();

		OnlyOnceAlteration() {}

		public ISpecification<T> Get(ISpecification<T> parameter) => new First().Allow<T>().And(parameter);
	}

	sealed class OnceAlteration<T> : IAlteration<ISpecification<T>>
	{
		public static OnceAlteration<T> Default { get; } = new OnceAlteration<T>();

		OnceAlteration() {}

		public ISpecification<T> Get(ISpecification<T> parameter) => new First<T>().And(parameter);
	}
}