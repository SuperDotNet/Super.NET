using Super.ExtensionMethods;
using Super.Text;
using System;
using System.Collections.Generic;

namespace Super.Model.Selection
{
	class NamedSelection<TParameter, TResult> : Decorated<string, Func<TParameter, TResult>>
	{
		public NamedSelection(ISelect<TParameter, TResult> @default,
		                      IEnumerable<KeyValuePair<string, Func<TParameter, TResult>>> options)
			: base(options.AsReadOnly()
			              .ToStore()
			              .In(NullOrEmptySelector.Default)
			              .ToSelect()
			              .Or(@default.ToDelegate().Accept)) {}
	}
}