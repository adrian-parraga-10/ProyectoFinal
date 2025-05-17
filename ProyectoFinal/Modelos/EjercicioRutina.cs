using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ProyectoFinal.Modelos
{
    public class EjercicioRutina
    {
        [BsonElement("ejercicio_id")]
        public ObjectId EjercicioId { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("categoria")]
        public string Categoria { get; set; }

        [BsonElement("serie_historial")]
        public List<SerieHistorial> SerieHistorial { get; set; }
    }

    public class SerieHistorial
    {
        [BsonElement("fecha_sesion")]
        public DateTime FechaSesion { get; set; }

        [BsonElement("series")]
        public List<Serie> Series { get; set; }
    }

    
}
