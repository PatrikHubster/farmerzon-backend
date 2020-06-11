using System.Net.Http;

namespace FarmerzonBackendManager.Implementation
{
    public abstract class AbstractManager
    {
        protected string FarmerzonArticles { get; private set; }
        protected IHttpClientFactory ClientFactory { get; set; }

        public AbstractManager(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
            FarmerzonArticles = "FarmerzonArticles";
        }
    }
}