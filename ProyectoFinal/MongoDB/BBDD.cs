using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

using ProyectoFinal.Modelos;

public class BBDD
{
    private readonly IMongoDatabase _database;
    private readonly string _connectionString = "mongodb+srv://adrianparraga1000:1234@cluster0.fvfhscl.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

    // Constructor que establece la conexión
    public BBDD()
    {
        var client = new MongoClient(_connectionString);
        _database = client.GetDatabase("fitnessApp"); // Nombre de la base de datos
    }

    // Método para obtener los alimentos desde la base de datos
    public async Task<List<Alimento>> ObtenerAlimentosAsync()
    {
        var collection = _database.GetCollection<Alimento>("alimentos");
        var alimentos = await collection.Find(Builders<Alimento>.Filter.Empty).ToListAsync();
        return alimentos;
    }

    // Método para obtener los usuarios desde la base de datos
    public async Task<Usuario> ObtenerUsuarioAsync(string email, string password)
    {
        var collection = _database.GetCollection<Usuario>("usuarios");
        var filtro = Builders<Usuario>.Filter.Eq("email", email) & Builders<Usuario>.Filter.Eq("contraseña", password);
        var usuario = await collection.Find(filtro).FirstOrDefaultAsync();
        return usuario;
    }

}
