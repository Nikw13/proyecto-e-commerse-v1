using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto_api_grado.Models
{
    public class Resena
    {
        public long IdResena { get; set; }
        public byte Rating { get; set; }
        public string Comentario { get; set; }
        public DateTime CreatedAt { get; set; }
        public long IdCliente { get; set; }
        public long IdProducto { get; set; }
    }
}