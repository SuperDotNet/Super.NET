using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using Serilog.Events;
using Serilog.Parsing;
using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Reflection;
using Super.Runtime.Activation;

namespace Super.Diagnostics.Logging
{
	sealed class Formats : ReferenceStore<MessageTemplate, IFormats>
	{
		public static Formats Default { get; } = new Formats();

		Formats() : base(x => x.Tokens
		                       .OfType<PropertyToken>()
		                       .ToDictionary(y => y.PropertyName, y => y.Format)
		                       .AsReadOnly()
		                       .To(I<Selection>.Default)) {}

		sealed class Selection : Select<string, string>, IFormats, IActivateMarker<IReadOnlyDictionary<string, string>>
		{
			readonly ImmutableArray<string> _names;

			[UsedImplicitly]
			public Selection(IReadOnlyDictionary<string, string> source)
				: this(source.ToStore().ToSelect(), source.Keys.ToImmutableArray()) {}

			public Selection(Func<string, string> source, ImmutableArray<string> names) : base(source) => _names = names;

			public ImmutableArray<string> Get() => _names;
		}
	}
}