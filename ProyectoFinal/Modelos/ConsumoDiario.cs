using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProyectoFinal.Modelos
{
    public class ConsumoDiario
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("usuarioId")]
        public ObjectId UsuarioId { get; set; }

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }

        [BsonElement("alimentosConsumidos")]
        public List<ConsumoAlimento> AlimentosConsumidos { get; set; } = new();
    }

}
