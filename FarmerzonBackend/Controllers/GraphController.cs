using System.Threading.Tasks;
using FarmerzonBackend.GraphControllerType;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmerzonBackend.Controllers
{
    [Authorize]
    [Route("graph")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private ISchema Schema { get; set; }
        private IDocumentExecuter Executer { get; set; }
        private DataLoaderDocumentListener DocumentListener { get; set; }

        public GraphController(ISchema schema, IDocumentExecuter executer, DataLoaderDocumentListener documentListener)
        {
            Schema = schema;
            Executer = executer;
            DocumentListener = documentListener;
        }
        
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] GraphQuery query)
        {
            var result = await Executer.ExecuteAsync(options =>
            {
                options.Schema = Schema;
                options.Query = query.Query;
                options.Listeners.Add(DocumentListener);
            }).ConfigureAwait(false);

            if(result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }
            
            return Ok(result.Data);
        }
    }
}