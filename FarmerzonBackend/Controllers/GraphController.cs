using System.Threading.Tasks;
using FarmerzonBackend.GraphControllerType;
using FarmerzonBackendManager.Interface;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmerzonBackend.Controllers
{
    [Authorize]
    [Route("graphql")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private ISchema Schema { get; set; }
        private IDocumentExecuter Executer { get; set; }
        private ITokenManager TokenManager { get; set; }
        private DataLoaderDocumentListener DocumentListener { get; set; }

        private string ExtractAccessToken()
        {
            var accessTokenRaw = HttpContext.Request.Headers["Authorization"];
            if (accessTokenRaw.Count != 1)
            {
                return null;
            }

            var accessTokenParts = accessTokenRaw[0].Split(" ");
            if (accessTokenParts.Length == 2)
            {
                return accessTokenParts[1];
            }

            return null;
        }

        public GraphController(ISchema schema, IDocumentExecuter executer, ITokenManager tokenManager, 
            DataLoaderDocumentListener documentListener)
        {
            Schema = schema;
            Executer = executer;
            TokenManager = tokenManager;
            DocumentListener = documentListener;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] GraphQuery query)
        {
            TokenManager.Token = ExtractAccessToken();
            var result = await Executer.ExecuteAsync(options =>
            {
                options.Schema = Schema;
                options.Query = query.Query;
                options.Listeners.Add(DocumentListener);
            }).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}