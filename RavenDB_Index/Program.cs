using Raven.Client.Documents;
using RavenDB_Index.Services;

namespace RavenDB_Index;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json");
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddScoped<IDocumentStore>(documentStore => InicializaStore(builder.Configuration));
        builder.Services.AddScoped<HttpClient>(client => InicializaHttpClient(builder.Configuration));
        builder.Services.AddScoped<IPopulaDocumentStore, PopulaDocumentStore>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static IDocumentStore InicializaStore(IConfiguration config)
    {
        var store = new DocumentStore()
        {
            Urls = new[] { config["RotaRavenDB"] },
            Database = config["NomeDoBanco"]
        };
        store.Initialize();
        return store;
    }
    
    private static HttpClient InicializaHttpClient(IConfiguration config)
    {
        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };
        var c = new HttpClient(handler);
        
        c.BaseAddress = new Uri(config["RotaAPI"]);
        
        return c;
    }
}