using MongoDB.Bson;

namespace ProyectoFinal.Modelos
{
    public class ConsumoAlimento
    {

        public ObjectId ConsumoId { get; set; }
        public ObjectId AlimentoId { get; set; }
        public string Nombre { get; set; }
        public int CantidadGramos { get; set; }
        public int Calorias { get; set; }
        public int Proteinas { get; set; }
        public int Carbohidratos { get; set; }
        public int Grasas { get; set; }
        public int Fibra { get; set; }



    }

}
