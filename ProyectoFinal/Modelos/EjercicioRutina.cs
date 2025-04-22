using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

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

    public class Serie
    {
        [BsonElement("serie_numero")]
        public int SerieNumero { get; set; }

        [BsonElement("repeticiones")]
        public int Repeticiones { get; set; }

        [BsonElement("peso")]
        public int Peso { get; set; }

        [BsonElement("comentarios")]
        public string Comentarios { get; set; }
    }
}
