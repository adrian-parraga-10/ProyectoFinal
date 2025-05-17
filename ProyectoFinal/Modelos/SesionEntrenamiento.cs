using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProyectoFinal.Modelos
{
    public class SesionEntrenamiento
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("usuario_id")]
        public ObjectId UsuarioId { get; set; }

        [BsonElement("fecha_sesion")]
        public DateTime FechaSesion { get; set; }

        [BsonElement("ejercicios")]
        public List<EjercicioSesion> Ejercicios { get; set; }

    }

    public class EjercicioSesion
    {
        [BsonElement("ejercicio_id")]
        public ObjectId EjercicioId { get; set; }

        [BsonIgnore] 
        public string Nombre { get; set; }

        [BsonElement("series")]
        public List<Serie> Series { get; set; }  
    }

    public class Serie
    {
        [BsonElement("serie_numero")]
        public int SerieNumero { get; set; }

        [BsonElement("repeticiones")]
        public int Repeticiones { get; set; }

        [BsonElement("peso")]
        public int Peso { get; set; }
    }
}
