using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using RavenDB_Index.Models;

namespace RavenDB_Index.Services;

public class PopulaDocumentStore : IPopulaDocumentStore
{
    public void PopulaStore(IDocumentStore store)
    {
        using (var session = store.OpenSession())
        {
            var produtos = CriaProdutos();
            
            foreach (var produto in produtos)
                session.Store(produto);
            
            session.SaveChanges();
        }

        using (var session = store.OpenSession())
        {
            var clientes = CriaClientes(session);

            foreach (var cliente in clientes)
                session.Store(cliente);
            
            session.SaveChanges();
        }
    }

    private static List<Cliente> CriaClientes(IDocumentSession session)
    {
        var nomes = new[] { "Nome1", "Nome2", "Nome3", "Nome4"};
        var sobrenomes = new[] { "Sobrenome1", "Sobrenome2", "Sobrenome3", "Sobrenome4" };

        var clientes = nomes
            .SelectMany(nome => sobrenomes, (nome, sobrenome) => new Cliente(nome, sobrenome))
            .ToList();
     
        var produtos = session.Advanced.LoadStartingWith<Produto>("produtos/").ToList();
        var random = new Random();
        
        foreach (var cliente in clientes)
        {
            var indexInicio = random.Next(0, produtos.Count - 1);
            var quantidade = random.Next(0, produtos.Count + 1 - indexInicio);
            cliente.AdicionaProdutos(produtos.GetRange(indexInicio, quantidade));
        }

        return clientes;
    }

    private static List<Produto> CriaProdutos()
    {
        var produtosValores = new Dictionary<string, decimal>()
        {
            { "Produto 1", 9.99m },
            { "Produto 2", 19.99m },
            { "Produto 3", 29.99m },
            { "Produto 4", 39.99m }
        };
        var marcas = new string[] { "Marca 1", "Marca 2", "Marca 3", "Marca 4"};

        return marcas
            .SelectMany(marca => produtosValores, (marca, produtoValor) => new Produto()
            {
                Descricao = $"{produtoValor.Key} da {marca}",
                Valor = produtoValor.Value
            })
            .ToList();
    }
}
