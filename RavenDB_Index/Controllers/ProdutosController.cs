using System.Web;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using RavenDB_Index.Data.IndexManagement.Indices.Produtos;
using RavenDB_Index.Indices;
using RavenDB_Index.Models;

namespace RavenDB_Index.Controllers;

[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    [HttpGet("")]
    public ActionResult<string> GetProdutos([FromServices] IDocumentStore store)
    {
        List<Produto> produtos = null;
        
        using (var session = store.OpenSession())
        {
            produtos = session
                .Advanced
                .LoadStartingWith<Produto>("produtos")
                .ToList();
        }

        return Ok(produtos);
    }

    [HttpGet("{idProduto}")]
    public ActionResult<string> GetProduto([FromServices] IDocumentStore store, [FromQuery(Name = "idProduto")] string idProduto)
    {
        idProduto = HttpUtility.UrlDecode(idProduto);
        Produto produto = null;

        using (var session = store.OpenSession())
        {
            produto = session.Load<Produto>(idProduto);
        }

        return Ok(produto);
    }

    [HttpGet("por-descricao")]
    public ActionResult<string> GetProdutoPorDescricao([FromServices] IDocumentStore store,
        [FromServices] HttpClient client, [FromQuery(Name = "descricao")] string descricao)
    {
        descricao = HttpUtility.UrlDecode(descricao);
        IEnumerable<Produto> produtos;

        ValidadorIndice.ValidaIndice(client,
            "RavenDB_Index.Data.IndexManagement.Indices.Produtos", 
            "Produtos_PorDescricao");

        using (var session = store.OpenSession())
        {
            produtos = Queryable.Where(session
                    .Query<Produto, Produtos_PorDescricao>(), p => p.Descricao == descricao)
                .ToList();
        }

        return Ok(produtos);
    }

    [HttpGet("por-valor")]
    public ActionResult<string> GetProdutoPorValor([FromServices] IDocumentStore store, [FromServices] HttpClient client,
        [FromQuery(Name = "maior-que")] decimal? maiorQue, [FromQuery(Name = "menor-que")] decimal? menorQue,
        [FromQuery(Name = "maior-ou-igual-a")] decimal? maiorOuIgualA, [FromQuery(Name = "menor-ou-igual-a")] decimal? menorOuIgualA)
    {
        IEnumerable<Produto> produtos;

        ValidadorIndice.ValidaIndice(client, "RavenDB_Index.Data.IndexManagement.Indices.Produtos", "Produtos_PorValor");
        
        using (var session = store.OpenSession())
        {
            var query = session.Query<Produto, Produtos_PorValor>();

            if (maiorQue.HasValue)
                query = query.Where(produto => produto.Valor > maiorQue);
            if (menorQue.HasValue)
                query = query.Where(produto => produto.Valor < menorQue);
            if (maiorOuIgualA.HasValue)
                query = query.Where(produto => produto.Valor >= maiorOuIgualA);
            if (menorOuIgualA.HasValue)
                query = query.Where(produto => produto.Valor <= menorOuIgualA);

            produtos = query.ToList();
        }

        return Ok(produtos);
    }

    [HttpPost("")]
    public ActionResult<string> PostProduto([FromServices] IDocumentStore store, [FromBody] Produto produto)
    {
        using (var session = store.OpenSession())
        {
            session.Store(produto);
            session.SaveChanges();
        }

        return Ok(produto);
    }
}