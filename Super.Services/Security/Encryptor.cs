using Super.Model.Selection.Alterations;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Super.Services.Security
{
	sealed class Encryptor : IAlteration<string>
	{
		readonly Encoding             _encoding;
		readonly ImmutableArray<byte> _key;

		public Encryptor(string key) : this(key, Encoding.ASCII) {}

		public Encryptor(string key, Encoding encoding) : this(encoding.GetBytes(key)
		                                                               .ToImmutableArray(), encoding) {}

		public Encryptor(ImmutableArray<byte> key, Encoding encoding)
		{
			_key      = key;
			_encoding = encoding;
		}

		public string Get(string parameter)
		{
			var key = _key;
			using (var hmac = new HMACSHA512(ImmutableArrayExtensions.ToArray(key)))
			{
				return BitConverter.ToString(hmac.ComputeHash(_encoding.GetBytes(parameter)))
				                   .Replace("-", string.Empty);
			}
		}
	}
}