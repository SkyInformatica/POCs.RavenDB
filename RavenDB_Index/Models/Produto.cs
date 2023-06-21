using System.ComponentModel.DataAnnotations;

namespace RavenDB_Index.Models;

public class Produto
{
    [Key]
    public string Id { get; private set; }
    public string? Descricao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataFabricacao { get; set; } = DateTime.Now;
}
