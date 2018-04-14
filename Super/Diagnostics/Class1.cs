using JetBrains.Annotations;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;
using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Runtime.Objects;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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

	public sealed class ProjectionsConfiguration : LoggerConfigurations
	{
		public static ProjectionsConfiguration Default { get; } = new ProjectionsConfiguration();

		ProjectionsConfiguration() : this(Projectors.Default, KnownProjectors.Default.Select(x => x.Key).ToImmutableArray()) {}

		public ProjectionsConfiguration(IProjectors projectors, ImmutableArray<Type> projectionTypes)
			: base(new SinkConfiguration(new ApplyProjectionsLogEventSink(projectors)).ToConfiguration(),
			       new ScalarConfiguration(projectionTypes.AsEnumerable()).ToConfiguration()) {}
	}

	sealed class ScalarConfiguration<T> : Delegated<LoggerDestructuringConfiguration, LoggerConfiguration>,
	                                      ILoggingDestructureConfiguration
	{
		public static ScalarConfiguration<T> Default { get; } = new ScalarConfiguration<T>();

		ScalarConfiguration() : base(x => x.AsScalar<T>()) {}
	}

	sealed class ScalarConfiguration : ILoggingDestructureConfiguration
	{
		readonly IEnumerable<Type> _types;

		public ScalarConfiguration(params Type[] types) : this(types.Hide()) {}

		public ScalarConfiguration(IEnumerable<Type> types) => _types = types;

		public LoggerConfiguration Get(LoggerDestructuringConfiguration parameter) => _types.Alter(parameter.AsScalar);
	}

	sealed class LoggerSinkSelector : Delegated<LoggerConfiguration, LoggerSinkConfiguration>
	{
		public static LoggerSinkSelector Default { get; } = new LoggerSinkSelector();

		LoggerSinkSelector() : base(x => x.WriteTo) {}
	}

	sealed class LoggerDestructureSelector : Delegated<LoggerConfiguration, LoggerDestructuringConfiguration>
	{
		public static LoggerDestructureSelector Default { get; } = new LoggerDestructureSelector();

		LoggerDestructureSelector() : base(x => x.Destructure) {}
	}

	sealed class SinkConfiguration : ILoggingSinkConfiguration
	{
		readonly ILogEventSink _sink;

		public SinkConfiguration(ILogEventSink sink) => _sink = sink;

		public LoggerConfiguration Get(LoggerSinkConfiguration parameter) => parameter.Sink(_sink);
	}

	sealed class ApplyProjectionsLogEventSink : ILogEventSink
	{
		readonly ISelect<MessageTemplate, IFormats> _format;
		readonly IProjectors                        _projectors;

		public ApplyProjectionsLogEventSink(IProjectors projectors) : this(Formats.Default, projectors) {}

		public ApplyProjectionsLogEventSink(ISelect<MessageTemplate, IFormats> format, IProjectors projectors)
		{
			_format     = format;
			_projectors = projectors;
		}

		public void Emit(LogEvent logEvent)
		{
			var formats    = _format.Get(logEvent.MessageTemplate);
			var properties = logEvent.Properties;
			foreach (var name in formats.Get())
			{
				var property = properties[name];
				if (property is ScalarValue scalar)
				{
					var projector = _projectors.Get(scalar.Value.GetType());
					if (projector != null)
					{
						var format     = formats.Get(name);
						var projection = projector(format)(scalar.Value);
						var value      = new ScalarValue(projection);
						logEvent.AddOrUpdateProperty(new LogEventProperty(name, value));
					}
				}
			}
		}
	}

	public interface IFormats : ISelect<string, string>, ISource<ImmutableArray<string>> {}

	sealed class Formats : ReferenceStore<MessageTemplate, IFormats>
	{
		public static Formats Default { get; } = new Formats();

		Formats() : base(x => x.Tokens
		                       .OfType<PropertyToken>()
		                       .ToDictionary(y => y.PropertyName, y => y.Format)
		                       .AsReadOnly()
		                       .To(I<Selection>.Default)) {}

		sealed class Selection : Delegated<string, string>, IFormats, IActivateMarker<IReadOnlyDictionary<string, string>>
		{
			readonly ImmutableArray<string> _names;

			[UsedImplicitly]
			public Selection(IReadOnlyDictionary<string, string> source)
				: this(source.ToStore().ToSelect(), source.Keys.ToImmutableArray()) {}

			public Selection(Func<string, string> source, ImmutableArray<string> names) : base(source) => _names = names;

			public ImmutableArray<string> Get() => _names;
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