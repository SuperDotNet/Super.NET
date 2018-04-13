using JetBrains.Annotations;
using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using Super.Runtime.Invocation.Expressions;
using Super.Text;
using Super.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Runtime.Objects
{
	public sealed class Projection : DynamicObject
	{
		readonly string                      _text;
		readonly IDictionary<string, object> _properties;

		public Projection(string text, Type instanceType, IEnumerable<KeyValuePair<string, object>> properties)
			: this(text, instanceType, properties.ToOrderedDictionary()) {}

		public Projection(string text, Type instanceType, IDictionary<string, object> properties)
		{
			_text        = text;
			_properties  = properties;
			InstanceType = instanceType;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (_properties.Keys.Contains(binder.Name))
			{
				result = _properties[binder.Name];
				return true;
			}

			return base.TryGetMember(binder, out result);
		}

		public override IEnumerable<string> GetDynamicMemberNames() => _properties.Keys;

		public Type InstanceType { get; }

		public override string ToString() => _text;
	}

	sealed class ApplicationDomainProjection : Projection<AppDomain>
	{
		public static ApplicationDomainProjection Default { get; } = new ApplicationDomainProjection();

		ApplicationDomainProjection()
			: base(DefaultApplicationDomainFormatter.Default, x => x.FriendlyName, x => x.Id) {}
	}

	sealed class ApplicationDomainProjections : Projections<AppDomain>
	{
		public static ApplicationDomainProjections Default { get; } = new ApplicationDomainProjections();

		ApplicationDomainProjections()
			: base(ApplicationDomainProjection.Default,
			       ApplicationDomainName.Default.Project(x => x.FriendlyName, x => x.Id, x => x.IsFullyTrusted),
			       ApplicationDomainIdentifier.Default.Project(x => x.FriendlyName, x => x.Id, x => x.BaseDirectory,
			                                                   x => x.RelativeSearchPath)) {}
	}

	class Projections<T> : TextSelect<T, Projection>
	{
		public Projections(ISelect<T, Projection> @default, params KeyValuePair<string, Func<T, Projection>>[] pairs) :
			base(@default, pairs) {}
	}

	public class Projection<T> : ISelect<T, Projection>
	{
		readonly Func<T, string>              _formatter;
		readonly ImmutableArray<IProperty<T>> _properties;

		public Projection(ISelect<T, string> formatter, params Expression<Func<T, object>>[] expressions)
			: this(formatter.ToDelegate(), expressions) {}

		public Projection(Func<T, string> formatter, params Expression<Func<T, object>>[] expressions)
			: this(formatter, expressions.Select(I<Property<T>>.Default.From).ToImmutableArray<IProperty<T>>()) {}

		public Projection(Func<T, string> formatter, ImmutableArray<IProperty<T>> expressions)
		{
			_formatter  = formatter;
			_properties = expressions;
		}

		public Projection Get(T parameter)
			=> new Projection(_formatter(parameter), parameter.GetType(), new Values(parameter, _properties));

		sealed class Values : ItemsBase<KeyValuePair<string, object>>
		{
			readonly T                            _subject;
			readonly ImmutableArray<IProperty<T>> _properties;

			public Values(T subject, ImmutableArray<IProperty<T>> properties)
			{
				_subject    = subject;
				_properties = properties;
			}

			public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				foreach (var property in _properties)
				{
					yield return property.Get(_subject);
				}
			}
		}
	}

	public interface IProperty<in T> : ISelect<T, KeyValuePair<string, object>> {}

	sealed class Property<T> : Decorated<T, KeyValuePair<string, object>>,
	                           IProperty<T>,
	                           IActivateMarker<Expression<Func<T, object>>>
	{
		[UsedImplicitly]
		public Property(Expression<Func<T, object>> expression)
			: base(new Invocation1<string, object, KeyValuePair<string, object>>(Pairs.Create,
			                                                                     expression.GetMemberInfo().Name)
				       .In(expression.Compile())) {}
	}
}