using Raven.Client.Documents.Indexes;
using RavenDB_Index.Models;

namespace RavenDB_Index.Data.IndexManagement.Indices.Produtos;

public class Produtos_PorDescricao : AbstractIndexCreationTask<Produto>
{
    public Produtos_PorDescricao()
    {
        Map = produtos => produtos
            .Select(produto => new
            {
                Descricao = produto.Descricao
            });
    }
}