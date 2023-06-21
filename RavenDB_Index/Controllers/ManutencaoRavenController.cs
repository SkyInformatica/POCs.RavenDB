using System.Web;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Indexes;
using RavenDB_Index.Services;

namespace RavenDB_Index.Controllers;

[Route("api/manutencao-raven")]
public class ManutencaoRavenController : ControllerBase
{
    [HttpPost("indices/{nomeIndice}")]
    public ActionResult<string> PostIndice([FromServices] IDocumentStore store, [FromRoute] string nomeIndice)
    {
        var tipoIndice = Type.GetType(nomeIndice)!;
        var indice = (IAbstractIndexCreationTask) Activator.CreateInstance(tipoIndice)!;

        indice.Execute(store);

        return Ok(nomeIndice);
    }

    [HttpPut("indices/ativa-indice/{nomeIndice}")]
    public ActionResult<string> AtivaIndice([FromServices] IDocumentStore store, [FromRoute] string nomeIndice)
    {
        nomeIndice = HttpUtility.UrlDecode(nomeIndice);
        var stats = store.Maintenance.Send(new GetStatisticsOperation());
        var indexStats = stats.Indexes.FirstOrDefault(indice => indice.Name == nomeIndice);

        if (indexStats == null)
            return NotFound("Índice não foi encontrado!");

        if (indexStats.State != IndexState.Normal)
        {
            store.Maintenance.Send(new EnableIndexOperation(nomeIndice));
            store.Maintenance.Send(new StartIndexOperation(nomeIndice));
        }
        
        return Ok(indexStats.State.ToString());
    }

    [HttpPost("popula-banco")]
    public ActionResult<string> PopulaBanco([FromServices] IDocumentStore store, [FromServices] IPopulaDocumentStore populaDocumentStore)
    {
        populaDocumentStore.PopulaStore(store);
        return Ok();
    }
}
