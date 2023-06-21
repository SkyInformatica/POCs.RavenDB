using Raven.Client.Documents.Indexes;
using RavenDB_Index.Models;

namespace RavenDB_Index.Indices.Clientes;

public class Clientes_PorProdutos : AbstractIndexCreationTask<Cliente>
{
    public Clientes_PorProdutos()
    {
        Map = clientes => clientes
            .SelectMany(cliente => cliente.Produtos
                .Select(produto => new
                {
                    IdProduto = produto.Id
                }));
    }
}