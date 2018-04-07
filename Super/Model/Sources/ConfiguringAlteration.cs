using Super.Model.Commands;
using Super.Model.Sources.Alterations;
using Super.Runtime.Activation;

namespace Super.Model.Sources
{
	public class ConfiguringAlteration<T> : IAlteration<T>, IActivateMarker<ICommand<T>>
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