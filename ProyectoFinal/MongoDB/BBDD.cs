using MongoDB.Bson;
using MongoDB.Driver;
using ProyectoFinal.Singleton;

using ProyectoFinal.Modelos;
using ProyectoFinal.Hash;

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
            _database = client.GetDatabase("fitnessApp"); 
            Console.WriteLine("Conexión exitosa a la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión a MongoDB: {ex.Message}");
        }
    }

    // Método para obtener los usuarios desde la base de datos
    public async Task<Usuario> ObtenerUsuarioAsync(string email, string contraseñaIngresada)
    {
        var collection = _database.GetCollection<Usuario>("usuarios");
        var usuario = await collection.Find(u => u.Email == email).FirstOrDefaultAsync();

        if (usuario != null && PasswordHelper.VerificarContraseña(contraseñaIngresada, usuario.Contraseña))
            return usuario;

        return null;
    }
    // Método para obtener los alimentos desde la base de datos
    public async Task<List<Alimento>> ObtenerAlimentosAsync()
    {
        var collection = _database.GetCollection<Alimento>("alimentos");
        var alimentos = await collection.Find(Builders<Alimento>.Filter.Empty).ToListAsync();
        return alimentos;
    }

    // Cargar solo los primeros N alimentos
    public async Task<List<Alimento>> ObtenerAlimentosLimitadoAsync(int limite)
    {
        var collection = _database.GetCollection<Alimento>("alimentos");
        var alimentos = await collection.Find(Builders<Alimento>.Filter.Empty)
                                        .Limit(limite)
                                        .ToListAsync();
        return alimentos;
    }

    public async Task<List<Alimento>> BuscarAlimentosAsync(string texto)
    {
        var collection = _database.GetCollection<Alimento>("alimentos");

        var filtro = Builders<Alimento>.Filter.Or(
            Builders<Alimento>.Filter.Regex("nombre", new MongoDB.Bson.BsonRegularExpression(texto, "i")),
            Builders<Alimento>.Filter.Regex("categoria", new MongoDB.Bson.BsonRegularExpression(texto, "i"))
        );

        return await collection.Find(filtro).Limit(30).ToListAsync(); 
    }



    


    // Método para obtener los ejercicios desde la base de datos
    public async Task<List<Ejercicio>> ObtenerEjerciciosAsync()
    {
        var collection = _database.GetCollection<Ejercicio>("ejercicios");
        var ejercicios = await collection.Find(Builders<Ejercicio>.Filter.Empty).ToListAsync();
        return ejercicios;
    }

    public async Task<List<Ejercicio>> ObtenerEjerciciosLimitadoAsync(int limite)
    {
        var collection = _database.GetCollection<Ejercicio>("ejercicios");
        var ejercicios = await collection.Find(Builders<Ejercicio>.Filter.Empty)
                                         .Limit(limite)
                                         .ToListAsync();
        return ejercicios;
    }

    public async Task<List<Ejercicio>> BuscarEjerciciosAsync(string texto)
    {
        var collection = _database.GetCollection<Ejercicio>("ejercicios");

        var filtro = Builders<Ejercicio>.Filter.Or(
            Builders<Ejercicio>.Filter.Regex("Nombre", new MongoDB.Bson.BsonRegularExpression(texto, "i")),
            Builders<Ejercicio>.Filter.Regex("Categoria", new MongoDB.Bson.BsonRegularExpression(texto, "i"))
        );

        return await collection.Find(filtro).Limit(30).ToListAsync();
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
                    ejercicioRutina.Nombre = ejercicio.Nombre;  
                   
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
    public async Task EliminarRutinaAsync(ObjectId rutinaId)
    {
        var collection = _database.GetCollection<Rutina>("rutinas");
        var filtro = Builders<Rutina>.Filter.Eq(r => r.Id, rutinaId);
        await collection.DeleteOneAsync(filtro);
    }

    public async Task GuardarSesionEntrenamientoAsync(SesionEntrenamiento sesion)
    {
        var collection = _database.GetCollection<SesionEntrenamiento>("sesiones");
        await collection.InsertOneAsync(sesion);
    }

    public async Task AgregarSerieAlHistorialAsync(ObjectId rutinaId, ObjectId ejercicioId, Serie nuevaSerie)
    {
        var collection = _database.GetCollection<Rutina>("rutinas");

        // Crear el filtro para encontrar la rutina y el ejercicio dentro
        var filtro = Builders<Rutina>.Filter.And(
            Builders<Rutina>.Filter.Eq(r => r.Id, rutinaId),
            Builders<Rutina>.Filter.ElemMatch(r => r.Ejercicios, e => e.EjercicioId == ejercicioId)
        );

        // Crear el update: agrega la serie al historial correspondiente
        var update = Builders<Rutina>.Update.Push("ejercicios.$.serie_historial", new SerieHistorial
        {
            FechaSesion = DateTime.Now,
            Series = new List<Serie> { nuevaSerie }
        });

        await collection.UpdateOneAsync(filtro, update);
    }


    public async Task<List<SerieHistorial>> ObtenerHistorialEjercicioAsync(ObjectId rutinaId, ObjectId ejercicioId)
    {
        var collection = _database.GetCollection<Rutina>("rutinas");

        var filtro = Builders<Rutina>.Filter.Eq(r => r.Id, rutinaId);
        var rutina = await collection.Find(filtro).FirstOrDefaultAsync();

        var ejercicio = rutina?.Ejercicios.FirstOrDefault(e => e.EjercicioId == ejercicioId);
        return ejercicio?.SerieHistorial ?? new List<SerieHistorial>();
    }

    

    public async Task<List<SesionEntrenamiento>> ObtenerSesionesPorUsuario(ObjectId usuarioId)
    {
        var collectionSesiones = _database.GetCollection<SesionEntrenamiento>("sesiones");
        var collectionEjercicios = _database.GetCollection<Ejercicio>("ejercicios");

        var filtro = Builders<SesionEntrenamiento>.Filter.Eq(s => s.UsuarioId, usuarioId);
        var sesiones = await collectionSesiones.Find(filtro).ToListAsync();

        foreach (var sesion in sesiones)
        {
            foreach (var ejercicioSesion in sesion.Ejercicios)
            {
                var ejercicio = await collectionEjercicios
                    .Find(e => e.Id == ejercicioSesion.EjercicioId)
                    .FirstOrDefaultAsync();

                if (ejercicio != null)
                {
                    ejercicioSesion.Nombre = ejercicio.Nombre;
                }
            }
        }

        return sesiones;
    }



    public async Task GuardarConsumoDiarioAsync(ConsumoDiario consumo)
    {
        consumo.Fecha = consumo.Fecha.Date.ToUniversalTime();  

        var collection = _database.GetCollection<ConsumoDiario>("consumos");
        await collection.InsertOneAsync(consumo);
    }


    public async Task AgregarAlimentoADiaAsync(ObjectId usuarioId, DateTime fecha, ConsumoAlimento alimento)
    {
        var collection = _database.GetCollection<ConsumoDiario>("consumos");

        var filtro = Builders<ConsumoDiario>.Filter.And(
            Builders<ConsumoDiario>.Filter.Eq(c => c.UsuarioId, usuarioId),
            Builders<ConsumoDiario>.Filter.Eq(c => c.Fecha, fecha.Date)
        );

        var update = Builders<ConsumoDiario>.Update.Push(c => c.AlimentosConsumidos, alimento);

        var options = new UpdateOptions { IsUpsert = true }; 

        await collection.UpdateOneAsync(filtro, update, options);
    }


    public async Task<ConsumoDiario> ObtenerConsumoDiarioPorFechaAsync(ObjectId usuarioId, DateTime fecha)
    {
        var collection = _database.GetCollection<ConsumoDiario>("consumos");

        var filtro = Builders<ConsumoDiario>.Filter.And(
            Builders<ConsumoDiario>.Filter.Eq(c => c.UsuarioId, usuarioId),
            Builders<ConsumoDiario>.Filter.Eq(c => c.Fecha, fecha.Date)
        );

        return await collection.Find(filtro).FirstOrDefaultAsync();
    }

    public async Task ReemplazarConsumoDiarioAsync(ConsumoDiario consumo)
    {
        var collection = _database.GetCollection<ConsumoDiario>("consumos");
        var filtro = Builders<ConsumoDiario>.Filter.Eq(c => c.Id, consumo.Id);
        await collection.ReplaceOneAsync(filtro, consumo);
    }


    public async Task<List<ConsumoDiario>> ObtenerConsumosPorUsuario(ObjectId usuarioId)
    {
        var collection = _database.GetCollection<ConsumoDiario>("consumos");
        var filtro = Builders<ConsumoDiario>.Filter.Eq(c => c.UsuarioId, usuarioId);
        var lista = await collection.Find(filtro).SortByDescending(c => c.Fecha).ToListAsync();
        return lista;
    }

    public async Task EliminarAlimentoDeDiaAsync(ObjectId usuarioId, DateTime fecha, ConsumoAlimento alimento)
    {
        var collection = _database.GetCollection<ConsumoDiario>("consumos");

        // Filtro para encontrar el documento de consumo del usuario y la fecha
        var filtro = Builders<ConsumoDiario>.Filter.And(
            Builders<ConsumoDiario>.Filter.Eq(c => c.UsuarioId, usuarioId),
            Builders<ConsumoDiario>.Filter.Eq(c => c.Fecha, fecha.Date)
        );

        // Operación de Pull para eliminar solo el alimento con el ConsumoId único
        var update = Builders<ConsumoDiario>.Update.PullFilter(
            c => c.AlimentosConsumidos,
            Builders<ConsumoAlimento>.Filter.Eq(a => a.ConsumoId, alimento.ConsumoId)
        );

        
        await collection.UpdateOneAsync(filtro, update);
    }

    public async Task<Usuario> ObtenerUsuarioPorIdAsync(ObjectId id)
    {
        var collection = _database.GetCollection<Usuario>("usuarios");
        return await collection.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task ActualizarUsuarioAsync(Usuario usuario)
    {
        var collection = _database.GetCollection<Usuario>("usuarios");
        var filtro = Builders<Usuario>.Filter.Eq(u => u.Id, usuario.Id);
        await collection.ReplaceOneAsync(filtro, usuario);
    }


    public async Task GuardarProgresoFisicoAsync(ProgresoFisico progreso)
    {
        var collection = _database.GetCollection<ProgresoFisico>("progresos");
        await collection.InsertOneAsync(progreso);
    }

    public async Task<List<ProgresoFisico>> ObtenerProgresosUsuarioAsync(ObjectId usuarioId)
    {
        var collection = _database.GetCollection<ProgresoFisico>("progresos");
        var filtro = Builders<ProgresoFisico>.Filter.Eq(p => p.UsuarioId, usuarioId);
        return await collection.Find(filtro).SortBy(p => p.Fecha).ToListAsync();
    }


    public async Task ActualizarPesoUsuarioAsync(ObjectId usuarioId, double nuevoPeso)
    {
        var collection = _database.GetCollection<Usuario>("usuarios");
        var update = Builders<Usuario>.Update.Set(u => u.Peso, nuevoPeso);
        await collection.UpdateOneAsync(u => u.Id == usuarioId, update);
    }



    public async Task InsertarAlimentoAsync(Alimento nuevo)
    {
        var collection = _database.GetCollection<Alimento>("alimentos");
        await collection.InsertOneAsync(nuevo);
    }

    public async Task ActualizarAlimentoAsync(Alimento alimento)
    {
        var collection = _database.GetCollection<Alimento>("alimentos");
        var filtro = Builders<Alimento>.Filter.Eq(a => a.Id, alimento.Id);
        await collection.ReplaceOneAsync(filtro, alimento);
    }

    public async Task EliminarAlimentoAsync(ObjectId id)
    {
        var collection = _database.GetCollection<Alimento>("alimentos");
        await collection.DeleteOneAsync(a => a.Id == id);
    }


    public async Task InsertarEjercicioAsync(Ejercicio nuevo)
    {
        var collection = _database.GetCollection<Ejercicio>("ejercicios");
        await collection.InsertOneAsync(nuevo);
    }

    public async Task ActualizarEjercicioAsync(Ejercicio ejercicio)
    {
        var collection = _database.GetCollection<Ejercicio>("ejercicios");
        var filtro = Builders<Ejercicio>.Filter.Eq(e => e.Id, ejercicio.Id);
        await collection.ReplaceOneAsync(filtro, ejercicio);
    }

    public async Task EliminarEjercicioAsync(string id)
    {
        var collection = _database.GetCollection<Ejercicio>("ejercicios");
        await collection.DeleteOneAsync(e => e.Id == ObjectId.Parse(id));
    }


    // Método para obtener todos los usuarios
    public async Task<List<Usuario>> ObtenerUsuariosAsync()
    {
        var collection = _database.GetCollection<Usuario>("usuarios");
        var usuarios = await collection.Find(Builders<Usuario>.Filter.Empty).ToListAsync();
        return usuarios;
    }

    // Método para insertar un nuevo usuario y devolver true si fue exitoso
    public async Task<bool> InsertarUsuarioAsync(Usuario usuario)
    {
        try
        {
            var collection = _database.GetCollection<Usuario>("usuarios");
            await collection.InsertOneAsync(usuario);
            return true; 
        }
        catch (Exception ex)
        {
            return false; 
        }
    }


    // Método para eliminar un usuario por su ID
    public async Task EliminarUsuarioAsync(ObjectId id)
    {
        var collection = _database.GetCollection<Usuario>("usuarios");
        var filtro = Builders<Usuario>.Filter.Eq("Id", id);
        await collection.DeleteOneAsync(filtro);
    }

    // Método para actualizar un usuario existente
    public async Task ActualizarAdminUsuarioAsync(Usuario usuario)
    {
        var collection = _database.GetCollection<Usuario>("usuarios");
        var filtro = Builders<Usuario>.Filter.Eq("Id", usuario.Id);
        var update = Builders<Usuario>.Update
            .Set(u => u.Nombre, usuario.Nombre)
            .Set(u => u.Email, usuario.Email)
            .Set(u => u.Edad, usuario.Edad)
            .Set(u => u.Sexo, usuario.Sexo)
            .Set(u => u.Peso, usuario.Peso)
            .Set(u => u.Altura, usuario.Altura);
        await collection.UpdateOneAsync(filtro, update);
    }
}



