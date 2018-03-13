using System.Collections.Generic;
using Super.Model.Commands;

namespace Super.Model.Sources
{
	public interface IAssignable<TKey, TValue> : ICommand<KeyValuePair<TKey, TValue>> {}
}