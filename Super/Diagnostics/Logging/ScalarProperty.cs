using Super.Model.Selection.Sources;
using Super.Runtime;

namespace Super.Diagnostics.Logging
{
	public readonly struct ScalarProperty : ISource<string>
	{
		readonly IFormats _formats;

		public ScalarProperty(string key, IFormats formats, object instance)
		{
			Key      = key;
			_formats = formats;
			Instance = instance;
		}

		public string Key { get; }

		public object Instance { get; }

		public string Get(Unit _) => _formats.Get(Key);
	}
}