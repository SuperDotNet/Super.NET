using JetBrains.Annotations;
using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using Super.Runtime.Invocation.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Runtime.Objects
{
	sealed class Values<T> : ItemsBase<KeyValuePair<string, object>>
	{
		readonly T                             _subject;
		readonly ImmutableArray<Definition<T>> _properties;

		public Values(T subject, ImmutableArray<Definition<T>> properties)
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

	sealed class Projection : DynamicObject
	{
		readonly OrderedDictionary<string, object> _properties;

		public Projection(Type instanceType, IEnumerable<KeyValuePair<string, object>> properties)
			: this(instanceType, new OrderedDictionary<string, object>(properties)) {}

		public Projection(Type instanceType, OrderedDictionary<string, object> properties)
		{
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
	}

	sealed class ApplicationDomainProjection : Projection<AppDomain>
	{
		public static ApplicationDomainProjection Default { get; } = new ApplicationDomainProjection();

		ApplicationDomainProjection() : base(x => x.FriendlyName, x => x.Id) {}
	}

	sealed class ApplicationDomainProjections : Projections<AppDomain>
	{
		public static ApplicationDomainProjections Default { get; } = new ApplicationDomainProjections();

		ApplicationDomainProjections() : base(ApplicationDomainProjection.Default, new Profiles<AppDomain>
		{
			{"Primary", new Projection<AppDomain>(x => x.FriendlyName, x => x.Id, x => x.IsFullyTrusted)},
			{"Diagnostic", new Projection<AppDomain>(x => x.FriendlyName, x => x.Id, x => x.BaseDirectory, x => x.RelativeSearchPath)}
		}) {}
	}

	class Projections<T> : NamedSelection<T, Projection>
	{
		public Projections(ISelect<T, Projection> @default, Profiles<T> profiles)
			: this(@default, profiles.ToOrderedDictionary(x => x.Key, x => x.Value.ToDelegate())) {}

		public Projections(ISelect<T, Projection> @default, IEnumerable<KeyValuePair<string, Func<T, Projection>>> options) : base(@default, options) {}
	}

	sealed class Profiles<T> : OrderedDictionary<string, Projection<T>> {}

	class Projection<T> : ISelect<T, Projection>
	{
		readonly ImmutableArray<Definition<T>> _properties;

		public Projection(params Expression<Func<T, object>>[] expressions)
			: this(expressions.Select(I<Definition<T>>.Default.From).ToImmutableArray()) {}

		public Projection(ImmutableArray<Definition<T>> expressions) => _properties = expressions;

		public Projection Get(T parameter) => new Projection(parameter.GetType(), new Values<T>(parameter, _properties));
	}

	sealed class Definition<T> : Decorated<T, KeyValuePair<string, object>>,
	                             IActivateMarker<Expression<Func<T, object>>>
	{
		[UsedImplicitly]
		public Definition(Expression<Func<T, object>> expression)
			: base(new Invocation1<string, object, KeyValuePair<string, object>>(Pairs.Create,
			                                                                     expression.GetMemberInfo().Name)
				       .In(expression.Compile())) {}
	}
}