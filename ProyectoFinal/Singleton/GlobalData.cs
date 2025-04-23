using MongoDB.Bson;
using ProyectoFinal.Modelos;

namespace ProyectoFinal.Singleton
{
    public class GlobalData
    {
        private static GlobalData _instance;
        public static GlobalData Instance => _instance ??= new GlobalData();

        // Propiedades para guardar el usuario actual e ID
        public ObjectId UsuarioId { get; set; }
        public Usuario UsuarioActual { get; set; }

        private GlobalData() { }
    }
}
