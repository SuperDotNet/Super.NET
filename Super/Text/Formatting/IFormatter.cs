using System;
using Super.Model.Selection.Conditions;

namespace Super.Text.Formatting
{
	public interface IFormatter : IConditional<object, IFormattable> {}
}