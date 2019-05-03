using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Reflection.Types;
using Super.Runtime.Environment;
using System;

namespace Super.Text.Formatting
{
	sealed class KnownFormatters : Select<object, IFormattable>
	{
		public static KnownFormatters Default { get; } = new KnownFormatters();

		KnownFormatters() : base(FormatterRegistration.Default.Get) {}
	}

	public interface IFormatter : IConditional<object, IFormattable> {}

	sealed class DefaultSystemFormatter : Conditional<object, IFormattable>, IFormatter
	{
		public static DefaultSystemFormatter Default { get; } = new DefaultSystemFormatter();

		DefaultSystemFormatter() : base(Always<object>.Default,
		                                Start.A.Selection.Of.Any.By.StoredActivation<DefaultFormatter>()) {}
	}

	public sealed class Formatter<T> : Formatter
	{
		public Formatter(ISelect<object, IFormattable> condition) : base(IsOf<T>.Default, condition) {}
	}

	public class Formatter : Conditional<object, IFormattable>, IFormatter
	{
		public Formatter(IConditional<object, IFormattable> condition)
			: this(condition.Condition, condition) {}

		public Formatter(ICondition<object> condition, ISelect<object, IFormattable> source)
			: base(condition, source) {}
	}

	public sealed class FormatterRegistration : SystemStore<IConditional<object, IFormattable>>
	{
		public static FormatterRegistration Default { get; } = new FormatterRegistration();

		FormatterRegistration() : base(DefaultSystemFormatter.Default.Self) {}
	}
}