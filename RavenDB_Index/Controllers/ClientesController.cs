using System.Web;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using RavenDB_Index.Indices;
using RavenDB_Index.Indices.Clientes;
using RavenDB_Index.Models;

namespace RavenDB_Index.Controllers;

[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    [HttpPost]
    public ActionResult<string> PostCliente([FromServices] IDocumentStore store, [FromBody] Cliente cliente)
    {
        using (var session = store.OpenSession())
        {
            session.Store(cliente);
            session.SaveChanges();
        }

        return Ok(cliente);
    }
    
    [HttpGet]
    public ActionResult<string> GetClientes([FromServices] IDocumentStore store)
    {
        Cliente[] clientes = null;
        
        using (var session = store.OpenSession())
        {
            clientes = session
                .Advanced
                .LoadStartingWith<Cliente>("clientes/");
        }

        return Ok(clientes);
    }

    [HttpGet("{idCliente}")]
    public ActionResult<string> GetCliente([FromServices] IDocumentStore store, [FromRoute] string idCliente)
    {
        idCliente = HttpUtility.UrlDecode(idCliente);
        Cliente cliente;

        using (var session = store.OpenSession())
            cliente = session.Query<Cliente>().FirstOrDefault(cliente => cliente.Id == idCliente);

        if (cliente == null)
            return NotFound("Cliente não encontrado!");
        return Ok(cliente);
    }

    [HttpGet("por-nome-e-sobrenome")]
    public ActionResult<string> GetClientePorNomeSobrenome([FromServices] IDocumentStore store,
        [FromServices] HttpClient client, [FromQuery(Name = "nome")] string nome,
        [FromQuery(Name = "sobrenome")] string sobrenome)
    {
        nome = HttpUtility.UrlDecode(nome);
        sobrenome = HttpUtility.UrlDecode(sobrenome);
        Cliente cliente;
        
        ValidadorIndice.ValidaIndice(client,
            "RavenDB_Index.Indices.Clientes",
            "Clientes_PorNomeSobrenome");

        using (var session = store.OpenSession())
        {
            cliente = session
                .Query<Cliente, Clientes_PorNomeSobrenome>()
                .FirstOrDefault(c =>
                    c.Nome == nome &&
                    c.Sobrenome == sobrenome)!;
        }

        if (cliente == null)
            return NotFound("Cliente não encontrado!");
        return Ok(cliente);
    }

    [HttpGet("por-produto")]
    public ActionResult<string> GetClientesPorProduto([FromServices] IDocumentStore store,
        [FromServices] HttpClient client, [FromQuery(Name = "produto")] string idProduto)
    {
        idProduto = HttpUtility.UrlDecode(idProduto);
        List<Cliente> clientes;
        
        ValidadorIndice.ValidaIndice(client,
            "RavenDB_Index.Indices.Clientes",
            "Clientes_PorProdutos");
        
        using (var session = store.OpenSession())
        {
            clientes = session
                .Query<Cliente, Clientes_PorProdutos>()
                .Where(c => c.Produtos.Any(p => p.Id == idProduto))
                .ToList();
        }

        return Ok(clientes);
    }

    [HttpPut("{idCliente}")]
    public ActionResult<string> PutCliente([FromServices] IDocumentStore store, [FromRoute] string idCliente, [FromBody] Cliente cliente)
    {
        idCliente = HttpUtility.UrlDecode(idCliente);
        Cliente clienteNoBanco;
        
        using (var session = store.OpenSession())
        {
            clienteNoBanco = session.Load<Cliente>(idCliente);

            if (clienteNoBanco == null)
                return NotFound("Cliente não encontrado!");
            
            clienteNoBanco.PutCliente(cliente);
            session.SaveChanges();
        }
        
        return Ok(clienteNoBanco);
    }

    [HttpDelete("{idCliente}")]
    public ActionResult<string> DeleteCliente([FromServices] IDocumentStore store, [FromRoute] string idCliente)
    {
        idCliente = HttpUtility.UrlDecode(idCliente);
        Cliente cliente = null;
        
        using (var session = store.OpenSession())
        {
            cliente = session.Load<Cliente>(idCliente);
            
            if (cliente == null)
                return NotFound("Cliente não encontrado!");
            
            session.Delete(cliente);
            session.SaveChanges();
        }
        
        return Ok("Cliente deletado com sucesso!");
    }
}