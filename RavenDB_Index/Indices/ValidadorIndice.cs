namespace RavenDB_Index.Indices;

public static class ValidadorIndice
{
    public static async void ValidaIndice(HttpClient client, string namespaceIndice, string nomeIndice)
    {
        await client.PostAsJsonAsync($"/api/manutencao-raven/indices/{namespaceIndice}.{nomeIndice}", "");
    }
}