using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


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
        public double Proteinas { get; set; }

        [BsonElement("carbohidratos")]
        public double Carbohidratos { get; set; }

        [BsonElement("grasas")]
        public double Grasas { get; set; }

        [BsonElement("fibra")]
        public double Fibra { get; set; }

        [BsonElement("origen")]
        public string Origen { get; set; }

        [BsonElement("imagen")]
        public string Imagen { get; set; }
    }
}