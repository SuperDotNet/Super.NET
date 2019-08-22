using System.Net.Http;
using Super.Model.Selection.Stores;

namespace Super.Application.Services.Communication
{
	sealed class AssociatedHandlers : ReferenceValueTable<HttpClient, System.Net.Http.HttpClientHandler>
	{
		public static AssociatedHandlers Default { get; } = new AssociatedHandlers();

		AssociatedHandlers() {}
	}
}