using System;
using Super.Compose;
using Super.Model.Selection.Conditions;

namespace Super.Text.Formatting
{
	sealed class DefaultSystemFormatter : Conditional<object, IFormattable>, IFormatter
	{
		public static DefaultSystemFormatter Default { get; } = new DefaultSystemFormatter();

		DefaultSystemFormatter() : base(Always<object>.Default,
		                                Start.A.Selection.Of.Any.By.StoredActivation<DefaultFormatter>()) {}
	}
}