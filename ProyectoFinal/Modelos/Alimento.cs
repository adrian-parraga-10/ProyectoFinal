using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Modelos
{
    public class Alimento
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("categoria")]
        public string Categoria { get; set; }

        [BsonElement("calorias")]
        public int Calorias { get; set; }

        [BsonElement("proteinas")]
        public int Proteinas { get; set; }

        [BsonElement("carbohidratos")]
        public int Carbohidratos { get; set; }

        [BsonElement("grasas")]
        public int Grasas { get; set; }

        [BsonElement("fibra")]
        public int Fibra { get; set; }

        [BsonElement("origen")]
        public string Origen { get; set; }
    }
}