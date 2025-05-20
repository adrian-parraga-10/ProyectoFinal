using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.ObjectModel;

public class Ejercicio
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("nombre")]
    public string Nombre { get; set; }

    [BsonElement("categoria")]
    public string Categoria { get; set; }

    [BsonElement("descripcion")]
    public string Descripcion { get; set; }

    [BsonElement("nivel")]
    public string Nivel { get; set; }

    [BsonElement("musculos_trabajados")]
    public ObservableCollection<string> MusculosTrabajados { get; set; } = new ObservableCollection<string>();

    [BsonElement("imagen")]
    public string Imagen { get; set; }

}
