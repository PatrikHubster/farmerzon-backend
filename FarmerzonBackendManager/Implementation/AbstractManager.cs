using System.Net.Http;
using FarmerzonBackendManager.Interface;

namespace FarmerzonBackendManager.Implementation
{
    public abstract class AbstractManager
    {
        protected string FarmerzonAddress { get; private set; }
        protected string FarmerzonArticles { get; private set; }
        protected IHttpClientFactory ClientFactory { get; set; }
        protected ITokenManager TokenManager { get; set; }

        public AbstractManager(IHttpClientFactory clientFactory, ITokenManager tokenManager)
        {
            ClientFactory = clientFactory;
            TokenManager = tokenManager;
            FarmerzonAddress = "FarmerzonAddress";
            FarmerzonArticles = "FarmerzonArticles";
        }
    }
}