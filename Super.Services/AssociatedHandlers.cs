using Super.Model.Selection.Stores;
using System.Net.Http;

namespace Super.Services
{
	sealed class AssociatedHandlers : ReferenceValueTable<HttpClient, System.Net.Http.HttpClientHandler>
	{
		public static AssociatedHandlers Default { get; } = new AssociatedHandlers();

		AssociatedHandlers() {}
	}
}