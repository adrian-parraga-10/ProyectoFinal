using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProyectoFinal.Modelos
{
    public class Usuario
    {
        [BsonId] // Asegúrate de que MongoDB reconozca este campo como el identificador
        public ObjectId Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("contraseña")]
        public string Contraseña { get; set; }

        [BsonElement("edad")]
        public int Edad { get; set; }

        [BsonElement("sexo")]
        public string Sexo { get; set; }

        [BsonElement("rol")]
        public string Rol { get; set; }

        [BsonElement("peso")]
        public double Peso { get; set; }

        [BsonElement("altura")]
        public double Altura { get; set; }

        [BsonElement("fecha_creación")]
        public DateTime FechaCreacion { get; set; }

        [BsonElement("objetivos")]
        public List<string> Objetivos { get; set; }

        [BsonElement("foto_perfil")]
        public string FotoPerfil { get; set; }

        [BsonElement("rutinas_guardadas")]
        public List<string> RutinasGuardadas { get; set; }

        [BsonElement("dieta_guardada")]
        public string DietaGuardada { get; set; }
    }
} 
