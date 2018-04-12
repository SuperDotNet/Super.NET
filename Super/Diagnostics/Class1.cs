using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Selection.Alterations;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Diagnostics
{
	sealed class ViewAwareSinkDecoration : LoggerSinkDecoration, IActivateMarker<ILoggingSinkConfiguration>
	{
		public ViewAwareSinkDecoration(ILoggingSinkConfiguration configuration) : base(I<ViewAwareSink>.Default.From,
		                                                                               configuration) {}
	}

	sealed class ViewAwareSink : SelectedParameterCommand<LogEvent, LogEvent>,
	                             ILogEventSink,
	                             IActivateMarker<ILogEventSink>
	{
		public ViewAwareSink(ILogEventSink sink) : base(sink.Emit, Implementations.ViewLogEvents) {}

		public void Emit(LogEvent logEvent)
		{
			Execute(logEvent);
		}
	}

	class LoggerSinkDecoration : IAlteration<LoggerConfiguration>
	{
		readonly Func<ILogEventSink, ILogEventSink> _sink;
		readonly Action<LoggerSinkConfiguration>    _configure;

		public LoggerSinkDecoration(IAlteration<ILogEventSink> sink, ILoggingSinkConfiguration configuration)
			: this(sink.ToDelegate(), configuration) {}

		public LoggerSinkDecoration(Func<ILogEventSink, ILogEventSink> sink, ILoggingSinkConfiguration configuration)
			: this(sink, configuration.ToCommand().ToDelegate()) {}

		public LoggerSinkDecoration(Func<ILogEventSink, ILogEventSink> sink, Action<LoggerSinkConfiguration> configure)
		{
			_sink      = sink;
			_configure = configure;
		}

		public LoggerConfiguration Get(LoggerConfiguration parameter)
			=> LoggerSinkConfiguration.Wrap(parameter.WriteTo, _sink, _configure);
	}

	public static class Implementations
	{
		public static Func<LogEvent, LogEvent> ViewLogEvents { get; } =
			Diagnostics.ViewLogEvents.Default.ToStore().ToDelegate();
	}

	sealed class ViewLogEvents : IAlteration<LogEvent>
	{
		public static ViewLogEvents Default { get; } = new ViewLogEvents();

		ViewLogEvents() {}

		public LogEvent Get(LogEvent parameter)
		{
			/*var properties = Properties(parameter.Properties).ToArray();
			var result = properties.Length > 0
				             ? new LogEvent(parameter.Timestamp, parameter.Level, parameter.Exception, parameter.MessageTemplate,
				                            properties)
				             : parameter;
			return result;*/
			return parameter;
		}

		/*static IEnumerable<LogEventProperty> Properties(IReadOnlyDictionary<string, LogEventPropertyValue> values)
		{
			var pairs  = values.ToArray();
			var length = pairs.Length;
			if (Any(pairs, length))
			{
				for (var i = 0; i < length; i++)
				{
					var pair = pairs[i];
					yield return new LogEventProperty(pair.Key,
					                                  pair.Value is ViewStructureValue view ? new ScalarValue(view.View) : pair.Value);
				}
			}
			yield break;
		}*/

		/*static bool Any(IReadOnlyList<KeyValuePair<string, LogEventPropertyValue>> pairs, int length)
		{
			for (var i = 0; i < length; i++)
			{
				var pair = pairs[i];
				if (pair.Value is ViewStructureValue)
				{
					return true;
				}
			}

			return false;
		}*/
	}

	/*sealed class ApplicationDomainPolicy : Policy<AppDomain>
	{
		public static ApplicationDomainPolicy Default { get; } = new ApplicationDomainPolicy();

		ApplicationDomainPolicy() : base(x => x.FriendlyName) {}
	}

	class Policy<T> : Policy
	{
		public Policy(params Expression<Func<T, object>>[] expressions)
			: base(typeof(T).Name, IsTypeSpecification<T>.Default,
			       new InstanceProperties<T>(expressions).In(Cast<object>.Default).Get) {}
	}

	class Policy : IDestructuringPolicy
	{
		readonly string                    _name;
		readonly ISpecification<object>    _specification;
		readonly Func<object, IProperties> _properties;

		public Policy(string name, ISpecification<object> specification, Func<object, IProperties> properties)
		{
			_name          = name;
			_specification = specification;
			_properties    = properties;
		}

		public bool TryDestructure(object instance, ILogEventPropertyValueFactory propertyValueFactory,
		                           out LogEventPropertyValue value)
		{
			var result = _specification.IsSatisfiedBy(instance);
			value = result ? new ViewStructureValue(instance, _properties(instance).Get(propertyValueFactory), _name) : null;
			return result;
		}
	}*/

	/*sealed class ViewStructureValue : StructureValue
	{
		public ViewStructureValue(object subject, IEnumerable<LogEventProperty> properties, string typeTag = null)
			: base(properties, typeTag) => View = subject;

		public object View { get; }
	}

	public interface IProperties : ISelect<ILogEventPropertyValueFactory, IEnumerable<LogEventProperty>> {}

	sealed class Properties : IProperties
	{
		readonly ImmutableArray<KeyValuePair<string, object>> _properties;

		public Properties(ImmutableArray<KeyValuePair<string, object>> properties) => _properties = properties;

		public IEnumerable<LogEventProperty> Get(ILogEventPropertyValueFactory parameter)
		{
			foreach (var property in _properties)
			{
				yield return new LogEventProperty(property.Key, parameter.CreatePropertyValue(property.Value));
			}
		}
	}*/


}
