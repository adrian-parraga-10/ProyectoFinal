using MongoDB.Bson;
using MongoDB.Driver;
using ProyectoFinal.Singleton;

using ProyectoFinal.Modelos;

public class BBDD
{
    private readonly IMongoDatabase _database;
    private readonly string _connectionString = "mongodb+srv://adrianparraga1000:1234@cluster0.fvfhscl.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

    // Constructor que establece la conexión
    public BBDD()
    {
        try
        {
            var client = new MongoClient(_connectionString);
            _database = client.GetDatabase("fitnessApp"); // Nombre de la base de datos
            Console.WriteLine("Conexión exitosa a la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión a MongoDB: {ex.Message}");
        }
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

    // Método para obtener los ejercicios desde la base de datos
    public async Task<List<Ejercicio>> ObtenerEjerciciosAsync()
    {
        var collection = _database.GetCollection<Ejercicio>("ejercicios");
        var ejercicios = await collection.Find(Builders<Ejercicio>.Filter.Empty).ToListAsync();
        return ejercicios;
    }

    // Método para obtener las rutinas
    public async Task<List<Rutina>> ObtenerRutinasUsuarioActualAsync()
    {
        var usuarioId = GlobalData.Instance.UsuarioId;
        var collectionRutinas = _database.GetCollection<Rutina>("rutinas");

        var filtro = Builders<Rutina>.Filter.Eq(r => r.UsuarioId, usuarioId);
        var rutinas = await collectionRutinas.Find(filtro).ToListAsync();

        return rutinas;
    }


    // Método para obtener una rutina con sus ejercicios
    public async Task<Rutina> ObtenerRutinaConEjercicios(ObjectId rutinaId)
    {
        var collectionRutinas = _database.GetCollection<Rutina>("rutinas");
        var rutina = await collectionRutinas.Find(r => r.Id == rutinaId).FirstOrDefaultAsync();

        if (rutina != null)
        {
            var collectionEjercicios = _database.GetCollection<Ejercicio>("ejercicios");

            // Para cada ejercicio en la rutina, obtener los detalles completos
            foreach (var ejercicioRutina in rutina.Ejercicios)
            {
                var ejercicio = await collectionEjercicios.Find(e => e.Id == ejercicioRutina.EjercicioId).FirstOrDefaultAsync();
                if (ejercicio != null)
                {
                    ejercicioRutina.Nombre = ejercicio.Nombre;  // Asignar nombre del ejercicio
                   
                }
            }
        }

        return rutina;
    }


    public async Task GuardarRutinaAsync(Rutina rutina)
    {
        var collection = _database.GetCollection<Rutina>("rutinas");
        await collection.InsertOneAsync(rutina);
    }




}
