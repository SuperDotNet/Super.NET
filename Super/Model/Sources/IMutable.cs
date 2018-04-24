using Super.Model.Commands;
using Super.Model.Extents;
using Super.Model.Specifications;
using Super.Runtime;
using System.Reactive;

namespace Super.Model.Sources
{
	public interface IMutable<T> : ISource<T>, ICommand<T> {}

	public class DecoratedMutable<T> : IMutable<T>
	{
		readonly IMutable<T> _mutable;

		public DecoratedMutable(IMutable<T> mutable) => _mutable = mutable;

		public T Get() => _mutable.Get();

		public void Execute(T parameter)
		{
			_mutable.Execute(parameter);
		}
	}

	public class Assignment<T> : DecoratedMutable<T>, ISpecification
	{
		readonly ISpecification<Unit> _specification;

		public Assignment(IMutable<T> mutable) : this(IsAssigned<T>.Default.In().In(mutable).Return(), mutable) {}

		public Assignment(ISpecification<Unit> specification, IMutable<T> mutable) : base(mutable)
			=> _specification = specification;

		public bool IsSatisfiedBy(Unit parameter) => _specification.IsSatisfiedBy(parameter);
	}
}