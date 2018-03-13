using Super.Model.Specifications;

namespace Super.Model.Commands
{
	class ValidatedCommand<T> : ICommand<T>
	{
		readonly ICommand<T>       _command;
		readonly ISpecification<T> _specification;

		public ValidatedCommand(ISpecification<T> specification, ICommand<T> command)
		{
			_specification = specification;
			_command       = command;
		}

		public void Execute(T parameter)
		{
			if (_specification.IsSatisfiedBy(parameter))
			{
				_command.Execute(parameter);
			}
		}
	}
}