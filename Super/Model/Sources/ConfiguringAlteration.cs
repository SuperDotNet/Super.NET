using Super.Model.Commands;
using Super.Model.Sources.Alterations;

namespace Super.Model.Sources
{
	public class ConfiguringAlteration<T> : IAlteration<T>
	{
		readonly ICommand<T> _configuration;

		public ConfiguringAlteration(ICommand<T> configuration) => _configuration = configuration;

		public T Get(T parameter)
		{
			_configuration.Execute(parameter);
			return parameter;
		}
	}
}