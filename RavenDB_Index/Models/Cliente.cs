using System.ComponentModel.DataAnnotations;

namespace RavenDB_Index.Models;

public class Cliente
{
    [Key]
    public string Id { get; private set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public DateTime DataCadastro { get; private set; } = DateTime.Now;
    public List<Produto> Produtos { get; private set; } = new List<Produto>();
    
    public Cliente(string nome, string sobrenome)
    {
        Nome = nome;
        Sobrenome = sobrenome;
    }

    public Cliente() { }

    public void PutCliente(Cliente cliente)
    {
        Nome = cliente.Nome;
        Sobrenome = cliente.Sobrenome;
        Produtos = cliente.Produtos;
    }

    public void AdicionaProduto(Produto produto)
    {
        Produtos.Add(produto);
    }

    public void AdicionaProdutos(IEnumerable<Produto> produtos)
    {
        foreach (var produto in produtos)
            AdicionaProduto(produto);
    }

    public void RemoveProduto(Produto produto)
    {
        Produtos.Remove(produto);
    }
}