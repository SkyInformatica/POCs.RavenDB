using Raven.Client.Documents.Indexes;
using RavenDB_Index.Models;

namespace RavenDB_Index.Data.IndexManagement.Indices.Produtos;

public class Produtos_PorValor : AbstractIndexCreationTask<Produto>
{
    public Produtos_PorValor()
    {
        Map = produtos => produtos
            .Select(produto => new
            {
                Valor = produto.Valor
            });
    }
}