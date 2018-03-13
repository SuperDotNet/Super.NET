using Super.Model.Specifications;

namespace Super.Model.Commands
{
	class ConditionalCommand<T> : ICommand<T>
	{
		readonly ICommand<T>       _false;
		readonly ISpecification<T> _specification;
		readonly ICommand<T>       _true;

		public ConditionalCommand(ISpecification<T> specification, ICommand<T> @true, ICommand<T> @false)
		{
			_specification = specification;
			_true          = @true;
			_false         = @false;
		}

		public void Execute(T parameter)
		{
			var command = _specification.IsSatisfiedBy(parameter) ? _true : _false;
			command.Execute(parameter);
		}
	}
}