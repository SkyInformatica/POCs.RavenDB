using Raven.Client.Documents.Indexes;
using RavenDB_Index.Models;

namespace RavenDB_Index.Indices.Clientes;

public class Clientes_PorProdutos : AbstractIndexCreationTask<Cliente, Clientes_PorProdutos.Result>
{
    public class Result
    {
        public string IdProduto { get; set; }
    }
    
    public Clientes_PorProdutos()
    {
        Map = clientes => clientes
            .SelectMany(cliente => cliente.Produtos, (cliente, produto) => new Result()
            {
                IdProduto = produto.Id
            });
    }
}