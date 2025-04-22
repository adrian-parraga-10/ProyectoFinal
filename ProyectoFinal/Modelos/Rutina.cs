using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ProyectoFinal.Modelos
{
    public class Rutina
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("descripcion")]
        public string Descripcion { get; set; }

        [BsonElement("usuario_id")]
        public ObjectId UsuarioId { get; set; }

        [BsonElement("publica")]
        public bool Publica { get; set; }

        [BsonElement("ejercicios")]
        public List<EjercicioRutina> Ejercicios { get; set; }

        [BsonElement("fecha_creación")]
        public DateTime FechaCreacion { get; set; }
    }
}

