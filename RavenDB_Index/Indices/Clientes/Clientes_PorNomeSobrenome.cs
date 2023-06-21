using Raven.Client.Documents.Indexes;
using RavenDB_Index.Models;

namespace RavenDB_Index.Indices.Clientes;

public class Clientes_PorNomeSobrenome : AbstractIndexCreationTask<Cliente>
{
    public Clientes_PorNomeSobrenome()
    {
        Map = clientes => clientes
            .Select(cliente => new
            {
                Nome = cliente.Nome,
                Sobrenome = cliente.Sobrenome
            });
    }
}
