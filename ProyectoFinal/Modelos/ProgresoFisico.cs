using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Modelos
{
    public class ProgresoFisico
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("usuario_id")]
        public ObjectId UsuarioId { get; set; }

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }

        [BsonElement("peso")]
        public double Peso { get; set; }

        [BsonElement("cintura")]
        public double? Cintura { get; set; }

        [BsonElement("pecho")]
        public double? Pecho { get; set; }

        [BsonElement("cadera")]
        public double? Cadera { get; set; }

        
    }

}
