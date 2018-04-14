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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Diagnostics
{
	sealed class ProjectionAwareSinkDecoration : LoggerSinkDecoration, IActivateMarker<ILoggingSinkConfiguration>
	{
		public ProjectionAwareSinkDecoration(ILogEventSink sink) : this(new SinkConfiguration(sink)) {}

		public ProjectionAwareSinkDecoration(ILoggingSinkConfiguration configuration)
			: base(I<ProjectionAwareSink>.Default.From, configuration) {}
	}

	sealed class ProjectionAwareSink : SelectedParameterCommand<LogEvent, LogEvent>,
	                                   IDisposable,
	                                   ILogEventSink,
	                                   IActivateMarker<ILogEventSink>
	{
		readonly IDisposable _disposable;

		public ProjectionAwareSink(ILogEventSink sink) : this(sink, sink.ToDisposable()) {}

		public ProjectionAwareSink(ILogEventSink sink, IDisposable disposable)
			: base(sink.Emit, Implementations.ViewLogEvents) => _disposable = disposable;

		public void Emit(LogEvent logEvent)
		{
			Execute(logEvent);
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}

	public sealed class ProjectionsConfiguration : LoggerConfigurations
	{
		public static ProjectionsConfiguration Default { get; } = new ProjectionsConfiguration();

		ProjectionsConfiguration() : this(Projectors.Default,
		                                  KnownProjectors.Default.Select(x => x.Key).ToImmutableArray()) {}

		public ProjectionsConfiguration(IProjectors projectors, ImmutableArray<Type> projectionTypes)
			: base(new SinkConfiguration(new ApplyProjectionsLogEventSink(projectors)).ToConfiguration(),
				   new EnrichmentConfiguration(PropertyFactories.Default).ToConfiguration(),
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

	sealed class LoggerEnrichmentSelector : Delegated<LoggerConfiguration, LoggerEnrichmentConfiguration>
	{
		public static LoggerEnrichmentSelector Default { get; } = new LoggerEnrichmentSelector();

		LoggerEnrichmentSelector() : base(x => x.Enrich) {}
	}

	sealed class LoggerDestructureSelector : Delegated<LoggerConfiguration, LoggerDestructuringConfiguration>
	{
		public static LoggerDestructureSelector Default { get; } = new LoggerDestructureSelector();

		LoggerDestructureSelector() : base(x => x.Destructure) {}
	}

	sealed class EnrichmentConfiguration : ILoggingEnrichmentConfiguration
	{
		readonly ImmutableArray<ILogEventEnricher> _enrichers;

		public EnrichmentConfiguration(params ILogEventEnricher[] enrichers) : this(enrichers.ToImmutableArray()) {}

		public EnrichmentConfiguration(ImmutableArray<ILogEventEnricher> enrichers) => _enrichers = enrichers;

		public LoggerConfiguration Get(LoggerEnrichmentConfiguration parameter) => parameter.With(_enrichers.ToArray());
	}

	sealed class SinkConfiguration : ILoggingSinkConfiguration
	{
		readonly ILogEventSink _sink;

		public SinkConfiguration(ILogEventSink sink) => _sink = sink;

		public LoggerConfiguration Get(LoggerSinkConfiguration parameter) => parameter.Sink(_sink);
	}

	sealed class ApplyProjectionsLogEventSink : ILogEventSink
	{
		readonly Func<LogEvent, IScalar> _scalars;
		readonly IProjectors             _projectors;

		public ApplyProjectionsLogEventSink(IProjectors projectors) : this(Implementations.Scalars, projectors) {}

		public ApplyProjectionsLogEventSink(Func<LogEvent, IScalar> scalars, IProjectors projectors)
		{
			_scalars    = scalars;
			_projectors = projectors;
		}

		public void Emit(LogEvent logEvent)
		{
			foreach (var scalar in _scalars(logEvent))
			{
				var instance  = scalar.Value.Instance;
				var projector = _projectors.Get(instance.GetType());
				if (projector != null)
				{
					var format     = scalar.Value.Get();
					var projection = projector(format)(instance);
					var value      = new ScalarValue(projection);
					logEvent.AddOrUpdateProperty(new LogEventProperty(scalar.Key, value));
				}
			}
		}
	}

	public interface IScalar : IReadOnlyDictionary<string, ScalarProperty> {}

	sealed class Scalar : IScalar
	{
		readonly IReadOnlyDictionary<string, ScalarProperty> _properties;

		public Scalar(IReadOnlyDictionary<string, ScalarProperty> properties) => _properties = properties;

		public IEnumerator<KeyValuePair<string, ScalarProperty>> GetEnumerator() => _properties.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_properties).GetEnumerator();

		public int Count => _properties.Count;

		public bool ContainsKey(string key) => _properties.ContainsKey(key);

		public bool TryGetValue(string key, out ScalarProperty value) => _properties.TryGetValue(key, out value);

		public ScalarProperty this[string key] => _properties[key];

		public IEnumerable<string> Keys => _properties.Keys;

		public IEnumerable<ScalarProperty> Values => _properties.Values;
	}

	public struct ScalarProperty : ISource<string>
	{
		readonly IFormats _formats;

		public ScalarProperty(string key, IFormats formats, object instance)
		{
			Key      = key;
			_formats = formats;
			Instance    = instance;
		}

		public string Key { get; }

		public object Instance { get; }

		public string Get() => _formats.Get(Key);
	}

	public interface IScalars : ISelect<LogEvent, IScalar> {}

	sealed class Scalars : IScalars
	{
		public static Scalars Default { get; } = new Scalars();

		Scalars() : this(Formats.Default) {}

		readonly ISelect<MessageTemplate, IFormats> _format;

		public Scalars(ISelect<MessageTemplate, IFormats> format) => _format = format;

		public IScalar Get(LogEvent parameter)
			=> new Scalar(Enumerate(parameter).ToDictionary(x => x.Key, x => x).AsReadOnly());

		IEnumerable<ScalarProperty> Enumerate(LogEvent parameter)
		{
			var formats    = _format.Get(parameter.MessageTemplate);
			var properties = parameter.Properties;
			foreach (var name in formats.Get())
			{
				var property = properties[name];
				if (property is ScalarValue scalar)
				{
					yield return new ScalarProperty(name, formats, scalar.Value);
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
		public static Func<LogEvent, IScalar> Scalars { get; } = Diagnostics.Scalars.Default.ToStore().ToDelegate();

		public static Func<LogEvent, LogEvent> ViewLogEvents { get; } =
			Diagnostics.ViewLogEvents.Default.ToStore().ToDelegate();
	}

	sealed class PropertyFactories : Decorated<LogEvent, ILogEventPropertyFactory>, ILogEventEnricher
	{
		public static PropertyFactories Default { get; } = new PropertyFactories();

		PropertyFactories() : this(ReferenceTables<LogEvent, ILogEventPropertyFactory>.Default.Get(x => null)) {}

		readonly ITable<LogEvent, ILogEventPropertyFactory> _table;

		public PropertyFactories(ITable<LogEvent, ILogEventPropertyFactory> table) : base(table) => _table = table;

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			_table.Assign(logEvent, propertyFactory);
		}
	}

	sealed class ViewLogEvents : IAlteration<LogEvent>
	{
		public static ViewLogEvents Default { get; } = new ViewLogEvents();

		ViewLogEvents() : this(Implementations.Scalars, PropertyFactories.Default.ToDelegate()) {}

		readonly Func<LogEvent, IScalar>                  _scalars;
		readonly Func<LogEvent, ILogEventPropertyFactory> _factories;

		public ViewLogEvents(Func<LogEvent, IScalar> scalars, Func<LogEvent, ILogEventPropertyFactory> factories)
		{
			_scalars   = scalars;
			_factories = factories;
		}

		public LogEvent Get(LogEvent parameter)
		{
			var properties = parameter.Properties;
			var structures = Structures(parameter, properties).ToDictionary(x => x.Name);
			var result = structures.Count > 0
				             ? new LogEvent(parameter.Timestamp, parameter.Level, parameter.Exception, parameter.MessageTemplate,
				                            Properties(properties, structures))
				             : parameter;
			return result;
		}

		static IEnumerable<LogEventProperty> Properties(IReadOnlyDictionary<string, LogEventPropertyValue> source,
		                                     IReadOnlyDictionary<string, LogEventProperty> structures)
		{
			var keys   = source.Keys.ToArray();
			var length = keys.Length;
			var result = new LogEventProperty[length];
			for (var i = 0; i < length; i++)
			{
				var key = keys[i];
				result[i] = structures.ContainsKey(key) ? structures[key] : new LogEventProperty(key, source[key]);
			}

			return result;
		}

		IEnumerable<LogEventProperty> Structures(LogEvent parameter,
		                                         IReadOnlyDictionary<string, LogEventPropertyValue> dictionary)
		{
			var factory = _factories(parameter);
			foreach (var scalar in _scalars(parameter))
			{
				if (dictionary[scalar.Key] is ScalarValue value && value.Value is Projection projection)
				{
					yield return new LogEventProperty(scalar.Key,
					                                  new StructureValue(Properties(projection, factory),
					                                                     projection.InstanceType.Name));
				}
			}
		}

		static IEnumerable<LogEventProperty> Properties(Projection projection, ILogEventPropertyFactory factory)
		{
			foreach (var name in projection)
			{
				yield return factory.CreateProperty(name.Key, name.Value, true);
			}
		}
	}
}