using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto_api_grado.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}