using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto_api_grado.Models
{
    public class CodigoPostal
    {
        public int IdCodigoPostal { get; set; }
        public string Codigo { get; set; }
        public bool Activo { get; set; }
        public int IdCiudad { get; set; }
    }
}