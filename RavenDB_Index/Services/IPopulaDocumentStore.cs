using Raven.Client.Documents;

namespace RavenDB_Index.Services;

public interface IPopulaDocumentStore
{
    public void PopulaStore(IDocumentStore store);
}
