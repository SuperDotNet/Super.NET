using System.Collections.Generic;

namespace Super.Model.Commands
{
	public interface IAssignable<TKey, TValue> : ICommand<KeyValuePair<TKey, TValue>> {}
}