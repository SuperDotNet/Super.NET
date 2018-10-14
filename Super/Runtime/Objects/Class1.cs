using JetBrains.Annotations;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using Super.Runtime.Invocation.Expressions;
using Super.Text;
using Super.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Runtime.Objects
{
	public interface IProjection : IReadOnlyDictionary<string, object>
	{
		Type InstanceType { get; }
	}

	public sealed class Projection : ReadOnlyDictionary<string, object>, IProjection
	{
		readonly string _text;

		/*public Projection(string text, Type instanceType, IEnumerable<KeyValuePair<string, object>> properties)
			: this(text, instanceType, properties.ToOrderedDictionary()) {}*/

		public Projection(string text, Type instanceType, IDictionary<string, object> properties) : base(properties)
		{
			_text        = text;
			InstanceType = instanceType;
		}

		public Type InstanceType { get; }

		public override string ToString() => _text;
	}

	sealed class KnownProjectors : ArrayInstance<KeyValuePair<Type, Func<string, Func<object, IProjection>>>>
	{
		public static KnownProjectors Default { get; } = new KnownProjectors();

		KnownProjectors() : this(ApplicationDomainProjection.Default.Entry()) {}

		public KnownProjectors(params KeyValuePair<Type, Func<string, Func<object, IProjection>>>[] items) :
			base(items) {}
	}

	static class Implementations
	{
		public static Func<Type, Func<string, Func<object, IProjection>>> KnownProjectors { get; }
			= Objects.KnownProjectors.Default.Select(x => x.AsEnumerable().ToStore().AsSelect()).Emit().Get;
	}

	public interface IProjectors : ISelect<Type, string, Func<object, IProjection>> {}

	sealed class Projectors : Select<Type, string, Func<object, IProjection>>, IProjectors
	{
		public static Projectors Default { get; } = new Projectors();

		Projectors() : base(Implementations.KnownProjectors) {}
	}

	sealed class ApplicationDomainProjection : FormattedProjection<AppDomain>
	{
		public static ApplicationDomainProjection Default { get; } = new ApplicationDomainProjection();

		ApplicationDomainProjection()
			: base(DefaultApplicationDomainFormatter.Default.Project(x => x.FriendlyName, x => x.Id),
			       ApplicationDomainName.Default.Entry(x => x.FriendlyName, x => x.Id, x => x.IsFullyTrusted),
			       ApplicationDomainIdentifier.Default.Entry(x => x.FriendlyName, x => x.Id, x => x.BaseDirectory,
			                                                 x => x.RelativeSearchPath)) {}
	}

	public interface IFormattedProjection<in T> : ISelect<string, T, IProjection> {}

	class FormattedProjection<T> : TextSelect<T, IProjection>, IFormattedProjection<T>
	{
		public FormattedProjection(ISelect<T, IProjection> @default,
		                           params KeyValuePair<string, Func<T, IProjection>>[] pairs)
			: base(@default, pairs) {}
	}

	public class Projection<T> : ISelect<T, IProjection>
	{
		readonly Func<T, string>                      _formatter;
		readonly Func<T, IDictionary<string, object>> _properties;

		public Projection(ISelect<T, string> formatter, params Expression<Func<T, object>>[] expressions)
			: this(formatter.ToDelegate(), expressions) {}

		public Projection(Func<T, string> formatter, params Expression<Func<T, object>>[] expressions)
			: this(formatter,
			       expressions.Select(I<Property<T>>.Default.From)
			                  .Result<IProperty<T>>()
			                  .To(I<Values>.Default)
			                  .Select(x => x.ToOrderedDictionary())
			                  .Get) {}

		public Projection(Func<T, string> formatter, Func<T, IDictionary<string, object>> properties)
		{
			_formatter  = formatter;
			_properties = properties;
		}

		public IProjection Get(T parameter)
			=> new Projection(_formatter(parameter), parameter.GetType(), _properties(parameter));

		// ReSharper disable once PossibleInfiniteInheritance
		sealed class Values : ISelect<T, IEnumerable<KeyValuePair<string, object>>>,
		                      IActivateMarker<Array<IProperty<T>>>
		{
			readonly Array<IProperty<T>> _properties;

			public Values(Array<IProperty<T>> properties) => _properties = properties;

			public IEnumerable<KeyValuePair<string, object>> Get(T parameter)
			{
				foreach (var property in _properties)
				{
					yield return property.Get(parameter);
				}
			}
		}
	}

	public interface IProperty<in T> : ISelect<T, KeyValuePair<string, object>> {}

	sealed class Property<T> : DecoratedSelect<T, KeyValuePair<string, object>>,
	                           IProperty<T>,
	                           IActivateMarker<Expression<Func<T, object>>>
	{
		[UsedImplicitly]
		public Property(Expression<Func<T, object>> expression)
			: this(expression,
			       new Invocation1<string, object, KeyValuePair<string, object>>(Pairs.Create,
			                                                                     expression.GetMemberInfo().Name)) {}

		[UsedImplicitly]
		public Property(Expression<Func<T, object>> expression, ISelect<object, KeyValuePair<string, object>> pairs)
			: base(expression.Compile().Out().Select(pairs)) {}
	}
}